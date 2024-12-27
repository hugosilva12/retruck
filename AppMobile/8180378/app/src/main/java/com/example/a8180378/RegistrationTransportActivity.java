package com.example.a8180378;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Spinner;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import com.example.a8180378.models.Status;
import com.example.a8180378.models.Transport;
import com.example.a8180378.models.TruckCategory;
import com.google.firebase.database.DatabaseReference;
import com.google.firebase.database.FirebaseDatabase;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.UUID;

/**
 * This activity allows the customer to register transports
 */
public class RegistrationTransportActivity extends AppCompatActivity implements AdapterView.OnItemSelectedListener {

    private DatabaseReference mDatabase;

    private Button register;

    private Spinner spinner;

    private TruckCategory truckCategory = null;

    private String username;

    private EditText date, origin, destiny, value, liters, weight, capacity;

    /**
     * This method instance the activity
     */
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.registration_transport_activity);
        date = findViewById(R.id.day);
        origin = findViewById(R.id.origin);
        destiny = findViewById(R.id.destiny);
        value = findViewById(R.id.value);
        liters = findViewById(R.id.litres);
        weight = findViewById(R.id.weight);
        capacity = findViewById(R.id.capacity);
        register = findViewById(R.id.login);

        Intent intent = getIntent();
        username = intent.getStringExtra("id");

        //Spinner
        ArrayAdapter<CharSequence> adapter = ArrayAdapter.createFromResource(this,
                R.array.truck_array, R.layout.item_spinner);
        adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        spinner = (Spinner) findViewById(R.id.spinner);
        spinner.setOnItemSelectedListener(this);
        spinner.setAdapter(adapter);

        register.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (truckCategory == null) {
                    Toast.makeText(RegistrationTransportActivity.this, "Tipo de camião Inválido", Toast.LENGTH_SHORT).show();
                } else if (verifyDate(date.getText().toString().trim()) == true) {
                    if (value.getText().toString().trim() != "") {
                        Transport transport = new Transport();
                        transport.setUserName(username);
                        transport.setDate(date.getText().toString().trim());
                        transport.setDestiny(destiny.getText().toString().trim());
                        transport.setOrigin(origin.getText().toString().trim());
                        if (!liters.getText().toString().trim().isEmpty()) {
                            transport.setLiters(Integer.parseInt(liters.getText().toString().trim()));
                        } else {
                            transport.setLiters(0);
                        }

                        if (!capacity.getText().toString().trim().isEmpty()) {
                            transport.setCapacity(Double.parseDouble(capacity.getText().toString().trim()));
                        } else {
                            transport.setCapacity(0.0);
                        }
                        if (!weight.getText().toString().trim().isEmpty()) {
                            transport.setWeight(Double.parseDouble(weight.getText().toString().trim()));
                        } else {
                            transport.setWeight(0.0);
                        }
                        transport.setValue_offered(0.0);
                        if(value.getText().toString() != null)
                            transport.setValue_offered(Double.parseDouble(value.getText().toString().trim()));

                        transport.setStatus(Status.WAIT_APROVE);
                        transport.setTruckCategory(truckCategory);

                        if (truckCategory == TruckCategory.REFRIGERATOR_TRUCK || truckCategory == TruckCategory.CONTAINER_TRUCK) {
                            if (capacity.getText().toString().trim().isEmpty()) {
                                Toast.makeText(RegistrationTransportActivity.this, "Tem de colocar a capacidade!", Toast.LENGTH_SHORT).show();
                            } else {
                                UUID uuid = UUID.randomUUID();
                                mDatabase = FirebaseDatabase.getInstance().getReference();
                                mDatabase.child("transports").child(uuid.toString()).setValue(transport);
                                Toast.makeText(RegistrationTransportActivity.this, "Registo Criado!", Toast.LENGTH_SHORT).show();
                            }
                        }

                        if (truckCategory == TruckCategory.CISTERN_TRUCK) {
                            if (liters.getText().toString().trim().isEmpty()) {
                                Toast.makeText(RegistrationTransportActivity.this, "Tem de colocar os litros!", Toast.LENGTH_SHORT).show();
                            } else {
                                UUID uuid = UUID.randomUUID();
                                mDatabase = FirebaseDatabase.getInstance().getReference();
                                mDatabase.child("transports").child(uuid.toString()).setValue(transport);
                                Toast.makeText(RegistrationTransportActivity.this, "Registo Criado!", Toast.LENGTH_SHORT).show();
                            }
                        }


                        if (truckCategory == TruckCategory.DUMP_TRUCK) {
                            if (weight.getText().toString().trim().isEmpty()) {
                                Toast.makeText(RegistrationTransportActivity.this, "Tem de colocar o peso!", Toast.LENGTH_SHORT).show();
                            } else {
                                //Cria
                                UUID uuid = UUID.randomUUID();
                                mDatabase = FirebaseDatabase.getInstance().getReference();
                                mDatabase.child("transports").child(uuid.toString()).setValue(transport);
                                Toast.makeText(RegistrationTransportActivity.this, "Registo Criado!", Toast.LENGTH_SHORT).show();
                            }
                        }

                    } else {
                        Toast.makeText(RegistrationTransportActivity.this, "Precisa de um valor de oferta!", Toast.LENGTH_SHORT).show();
                    }

                } else {
                    Toast.makeText(RegistrationTransportActivity.this, "Data Invalida!", Toast.LENGTH_SHORT).show();
                }
            }
        });
    }

    /**
     * This method selects the truck category through the spinner
     */
    @Override
    public void onItemSelected(AdapterView<?> adapterView, View view, int i, long l) {
        String text = adapterView.getItemAtPosition(i).toString();
        if (i == 1) {
            truckCategory = TruckCategory.REFRIGERATOR_TRUCK;
        } else if (i == 2) {
            truckCategory = TruckCategory.DUMP_TRUCK;
        } else if (i == 3) {
            truckCategory = TruckCategory.CISTERN_TRUCK;
        } else if (i == 4) {
            truckCategory = TruckCategory.CONTAINER_TRUCK;
        } else {
            truckCategory = null;
        }

    }

    /**
     * This method checks if the date entered by the user is valid
     *
     * @param date date to validate
     * @return true if date is valid, false if not
     */
    public boolean verifyDate(String date) {
        String[] dateSplit = date.split("/");
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
            date1 = new SimpleDateFormat("dd/MM/yyyy").parse(date);
            date1.setHours(23);
            date1.setMinutes(59);
            date1.setSeconds(40);
            Date today = new Date();
            if (date1.compareTo(today) < 0) {
                Toast.makeText(RegistrationTransportActivity.this, "Data já passou", Toast.LENGTH_SHORT).show();
                return false;
            }
        } catch (ParseException e) {
            e.printStackTrace();
        }

        return true;
    }

    @Override
    public void onNothingSelected(AdapterView<?> adapterView) {

    }
}
