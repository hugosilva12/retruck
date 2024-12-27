package com.example.a8180378;

import android.Manifest;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.graphics.Color;
import android.location.Location;
import android.os.Bundle;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.app.ActivityCompat;

import com.example.a8180378.models.Coor;
import com.example.a8180378.models.Service;
import com.example.a8180378.models.ServiceCoor;
import com.example.a8180378.models.ServicesStatus;
import com.google.android.gms.location.FusedLocationProviderClient;
import com.google.android.gms.location.LocationServices;
import com.google.android.gms.tasks.OnFailureListener;
import com.google.android.gms.tasks.OnSuccessListener;
import com.google.android.material.bottomnavigation.BottomNavigationView;
import com.google.firebase.database.DatabaseReference;
import com.google.firebase.database.FirebaseDatabase;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.Serializable;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.UUID;

/**
 * This activity allows the driver to update the status of the service
 */
public class ServicesActivity extends AppCompatActivity implements BottomNavigationView.OnNavigationItemSelectedListener {
    private DatabaseReference mDatabase;
    private String username;
    private Service service;
    private String idToUpdateService = "";
    private Coor coor = new Coor();
    private Button upload;
    private Button finishService;
    private Button initService;
    private FusedLocationProviderClient mFusedLocationClient;
    private static final int REQUEST_FINE_LOCATION = 100;

    private ArrayList<Object> servicesIds = new ArrayList<>();
    private ArrayList<Object> services = new ArrayList<>();
    private BottomNavigationView navigationView;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.services_activity);
        upload = findViewById(R.id.login);
        finishService = findViewById(R.id.button_final);
        initService = findViewById(R.id.init);
        navigationView = (BottomNavigationView) findViewById(R.id.navigationView);
        navigationView.setOnNavigationItemSelectedListener(ServicesActivity.this);

        //GetUserName
        Intent intent = getIntent();
        username = intent.getStringExtra("id");
        Bundle args = intent.getBundleExtra("usernames");
        Bundle args2 = intent.getBundleExtra("data");
        services = (ArrayList<Object>) args.getSerializable("ARRAYLIST");
        servicesIds = (ArrayList<Object>) args2.getSerializable("ARRAYLIST_IDS");

        for (int i = 0; i < servicesIds.size(); i++) {
            System.out.println("SERVICE ID  " + servicesIds.get(i).toString());
        }
        int count = 0;
        for (Object ob : services) {

            try {
                JSONObject jo = new JSONObject(ob.toString());

                if (jo.getString("userNameDriver").equals(username) && verifyDate(jo.getString("date"))) {
                    //if (jo.getString("status") != null && jo.getString("status").equals(ServicesStatus.IN_PROGRESS)) {
                    service = new Service();
                    service.setUserNameDriver(username);
                    idToUpdateService = servicesIds.get(count).toString();
                    service.setIdService(jo.getString("idService"));
                    service.setDate(jo.getString("date"));
                    service.setDestiny(jo.getString("destiny"));
                    service.setOrigin(jo.getString("origin"));


                    if (jo.getString("status").equals("1") || jo.getString("status").equals("2") || jo.getString("status").equals("0")) {
                        if (Integer.valueOf(jo.getString("status")) == 0) {
                            service.setStatus(ServicesStatus.TO_START);
                        } else if (Integer.valueOf(jo.getString("status")) == 1) {
                            service.setStatus(ServicesStatus.IN_PROGRESS);
                        } else {
                            service.setStatus(ServicesStatus.FINISHED);
                        }
                    } else {
                        service.setStatus(ServicesStatus.valueOf(jo.getString("status")));
                    }

                }
            } catch (JSONException e) {
                e.printStackTrace();
            }
            count++;
        }
        if (service != null) {
            if (service.getStatus() != ServicesStatus.TO_START) {
                disableInitButton();
            }
            if (service.getStatus() == ServicesStatus.FINISHED) {
                updateButtons("Serviço Finalizado");
            }
        }


        initService.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                startService();
            }
        });

        upload.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                getLocation();
            }
        });
        finishService.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                finishService();
            }
        });

        if (idToUpdateService == "") {
            updateButtons("Não  tem Serviço");
        }

    }

    /**
     * This method updates service state to IN_PROGRESS
     */
    private void startService() {
        mDatabase = FirebaseDatabase.getInstance().getReference();
        service.setStatus(ServicesStatus.IN_PROGRESS);
        mDatabase.child("services").child(idToUpdateService).setValue(service);
        Toast.makeText(ServicesActivity.this, "Iniciado com Sucesso!", Toast.LENGTH_SHORT).show();
        disableInitButton();
    }

    /**
     * This method updates service state to FINISHED
     */
    private void finishService() {
        mDatabase = FirebaseDatabase.getInstance().getReference();
        service.setStatus(ServicesStatus.FINISHED);
        mDatabase.child("services").child(idToUpdateService).setValue(service);
        Toast.makeText(ServicesActivity.this, "Enviada com Sucesso!", Toast.LENGTH_SHORT).show();
        updateButtons("Serviço Finalizado");
    }

    /**
     * This method updates the state of init button
     */
    private void disableInitButton() {
        initService.setEnabled(false);
        initService.setBackgroundColor(Color.WHITE);
        initService.setText("Serviço em Progesso");
    }

    /**
     * This method updates the state of buttons
     */
    private void updateButtons(String desc) {
        upload.setEnabled(false);
        upload.setBackgroundColor(Color.WHITE);
        initService.setTextColor(Color.BLACK);
        initService.setText(desc);
        initService.setEnabled(false);
        initService.setBackgroundColor(Color.WHITE);
        finishService.setVisibility(View.INVISIBLE);
        finishService.setText("");
    }

    /**
     * This method asks for permission to access the cell phone location
     */
    private void requestPermissions() {
        ActivityCompat.requestPermissions(this, new String[]{Manifest.permission.ACCESS_FINE_LOCATION}, REQUEST_FINE_LOCATION);

    }

    /**
     * This method gets the location of the mobile phone
     */
    public void getLocation() {
        if (ActivityCompat.checkSelfPermission(this, Manifest.permission.ACCESS_FINE_LOCATION) != PackageManager.PERMISSION_GRANTED) {
            requestPermissions();
        }
        mFusedLocationClient = LocationServices.getFusedLocationProviderClient(ServicesActivity.this);
        mFusedLocationClient.getLastLocation().addOnSuccessListener(ServicesActivity.this, new OnSuccessListener<Location>() {
            @Override
            public void onSuccess(Location location) {
                if (location != null) {
                    location.getLatitude();
                    coor.setLongitude(Float.parseFloat(String.valueOf(location.getLongitude())));
                    coor.setLatitude(Float.parseFloat(String.valueOf(location.getLatitude())));
                    ServiceCoor serviceCoor = new ServiceCoor();
                    serviceCoor.setCoord(coor);
                    serviceCoor.setIdService(service.getIdService());
                    UUID uuid = UUID.randomUUID();
                    mDatabase = FirebaseDatabase.getInstance().getReference();
                    mDatabase.child("location_services").child(uuid.toString()).setValue(serviceCoor);
                    Toast.makeText(ServicesActivity.this, "Enviada com Sucesso!", Toast.LENGTH_SHORT).show();
                }else{
                    Toast.makeText(ServicesActivity.this, "Falha a obter no dispositivo!", Toast.LENGTH_SHORT).show();
                }

            }
        }).addOnFailureListener(ServicesActivity.this, new OnFailureListener() {
            @Override
            public void onFailure(@NonNull Exception e) {
                Toast.makeText(ServicesActivity.this, "Falha a enviar a localização ", Toast.LENGTH_SHORT).show();
            }
        });
    }
    /**
     * This method checks if a date is today
     */
    public boolean verifyDate(String dateToConvert){
        String [] dateSplit = dateToConvert.split("-");
        if (dateSplit.length != 3) {
            return false;
        } else if (dateSplit[0].length() != 2) {
            return false;
        } else if (dateSplit[1].length() != 2) {
            return false;
        } else if (dateSplit[2].length() != 4) {
            return false;
        }
        Date date1 = null;

        try {
             date1 = new SimpleDateFormat("dd-MM-yyyy").parse(dateToConvert);
        } catch (ParseException e) {
            e.printStackTrace();
        }
        Date today = new Date();
        date1.setHours(23);
        date1.setMinutes(59);
        date1.setSeconds(40);
        today.setHours(23);
        today.setMinutes(59);
        today.setSeconds(40);
        String date1s = today.toString();
        String date2s = date1.toString();
        if(date1s.equals(date2s)){
            return true;
        }
        return false;
    }
    /**
     * Method to control the menu
     */
    @Override
    public boolean onNavigationItemSelected(@NonNull MenuItem menuItem) {

        switch (menuItem.getItemId()) {
            case R.id.absence: {
                Intent i = new Intent(ServicesActivity.this, AbsenceActivity.class);
                i.putExtra("id", username);
                i.putExtra("data", servicesIds.toString());
                Bundle args = new Bundle();
                Bundle args_2 = new Bundle();
                args.putSerializable("ARRAYLIST", (Serializable) services);
                args_2.putSerializable("ARRAYLIST_IDS", (Serializable) servicesIds);
                i.putExtra("usernames", args);
                i.putExtra("data", args_2);
                startActivity(i);
                break;

            }
            case R.id.home: {
                Intent i = new Intent(ServicesActivity.this, HomeActivity.class);
                i.putExtra("id", username);
                i.putExtra("data", servicesIds.toString());
                Bundle args = new Bundle();
                Bundle args_2 = new Bundle();
                args.putSerializable("ARRAYLIST", (Serializable) services);
                args_2.putSerializable("ARRAYLIST_IDS", (Serializable) servicesIds);
                i.putExtra("usernames", args);
                i.putExtra("data", args_2);
                startActivity(i);
                break;
            }
            case R.id.services: {
                break;
            }
        }
        return true;
    }
}


