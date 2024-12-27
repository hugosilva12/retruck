package com.example.a8180378;

import android.content.Intent;
import android.os.Bundle;
import android.view.MenuItem;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Spinner;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;

import com.example.a8180378.models.Absence;
import com.example.a8180378.models.AbsenceType;
import com.google.android.material.bottomnavigation.BottomNavigationView;
import com.google.firebase.database.DatabaseReference;
import com.google.firebase.database.FirebaseDatabase;

import java.io.Serializable;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.UUID;

/**
 * AbsenceActivity for drivers register your absences
 */
public class AbsenceActivity extends AppCompatActivity implements BottomNavigationView.OnNavigationItemSelectedListener, AdapterView.OnItemSelectedListener {

    private String username;
    private Button absence;
    private EditText date, description;
    private ArrayList<Object> servicesIds = new ArrayList<>();
    private ArrayList<Object> services = new ArrayList<>();
    private BottomNavigationView navigationView;
    private Spinner spinner;
    private AbsenceType absenceType = null;
    private DatabaseReference mDatabase;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.absence_activity);
        absence = findViewById(R.id.button_final);
        navigationView = (BottomNavigationView) findViewById(R.id.navigationView);
        navigationView.setOnNavigationItemSelectedListener(AbsenceActivity.this);
        date = findViewById(R.id.day);
        description = findViewById(R.id.description);

        //Spinner
        ArrayAdapter<CharSequence> adapter = ArrayAdapter.createFromResource(this,
                R.array.planets_array, R.layout.item_spinner);
        adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        spinner = (Spinner) findViewById(R.id.spinner);
        spinner.setOnItemSelectedListener(this);
        spinner.setAdapter(adapter);

        //GetUserName
        Intent intent = getIntent();
        username = intent.getStringExtra("id");
        Bundle args = intent.getBundleExtra("usernames");
        Bundle args2 = intent.getBundleExtra("data");
        services = (ArrayList<Object>) args.getSerializable("ARRAYLIST");
        servicesIds = (ArrayList<Object>) args2.getSerializable("ARRAYLIST_IDS");

        absence.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (absenceType == null) {
                    Toast.makeText(AbsenceActivity.this, "Tipo Inválido Inválidos", Toast.LENGTH_SHORT).show();
                } else {
                    if (verifyDate(date.getText().toString().trim()) == true) {
                        Absence absence = new Absence();
                        absence.setId(username);
                        absence.setDate(date.getText().toString().trim());
                        absence.setAbsenceType(absenceType);
                        absence.setDescription(description.getText().toString().trim());
                        UUID uuid = UUID.randomUUID();
                        mDatabase = FirebaseDatabase.getInstance().getReference();
                        mDatabase.child("absences").child(uuid.toString()).setValue(absence);
                        Toast.makeText(AbsenceActivity.this, "Absence Criada!", Toast.LENGTH_SHORT).show();
                    } else {
                        Toast.makeText(AbsenceActivity.this, "Data Invalida", Toast.LENGTH_SHORT).show();
                    }

                }
            }
        });

    }

    /**
     * Method to control the menu
     */
    @Override
    public boolean onNavigationItemSelected(@NonNull MenuItem menuItem) {

        switch (menuItem.getItemId()) {
            case R.id.absence: {
                break;

            }
            case R.id.home: {
                Intent i = new Intent(AbsenceActivity.this, HomeActivity.class);
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
                Intent i = new Intent(AbsenceActivity.this, ServicesActivity.class);
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
        }
        return true;
    }

    /**
     * This method selects the absence category through the spinner
     */
    @Override
    public void onItemSelected(AdapterView<?> adapterView, View view, int i, long l) {
        String text = adapterView.getItemAtPosition(i).toString();
        if (i == 1) {
            absenceType = AbsenceType.FAMILY;
        } else if (i == 2) {
            absenceType = AbsenceType.SICK;
        } else if (i == 3) {
            absenceType = AbsenceType.VACATION;
        } else {
            absenceType = null;
        }

    }

    @Override
    public void onNothingSelected(AdapterView<?> adapterView) {

    }

    /**
     * This method checks if the date entered by the user is valid
     * @param date date to validate
     * @return true if date is valid, false if not
     */
    public boolean verifyDate(String date) {
        String[] dateSplit = date.split("/");
        if (dateSplit.length != 3) {
            return false;
        }else if(dateSplit[0].length() != 2){
            return false;
        }else if(dateSplit[1].length() != 2){
            return false;
        } else if(dateSplit[2].length() != 4){
            return false;
        }
        Date date1= null;
        try {
            date1 = new SimpleDateFormat("dd/MM/yyyy").parse(date);
            date1.setHours(23);
            date1.setMinutes(59);
            date1.setSeconds(40);
            Date today = new Date();
            if(date1.compareTo(today) < 0) {
                Toast.makeText(AbsenceActivity.this, "Data já passou", Toast.LENGTH_SHORT).show();
                return false;
            }
        } catch (ParseException e) {
            e.printStackTrace();
        }

        return true;
    }
}