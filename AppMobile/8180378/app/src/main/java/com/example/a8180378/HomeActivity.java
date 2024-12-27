package com.example.a8180378;

import android.content.Intent;
import android.graphics.Color;
import android.os.Bundle;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;

import com.google.android.gms.tasks.OnCompleteListener;
import com.google.android.gms.tasks.Task;
import com.google.android.material.bottomnavigation.BottomNavigationView;
import com.google.firebase.database.DataSnapshot;
import com.google.firebase.database.DatabaseReference;
import com.google.firebase.database.FirebaseDatabase;

import java.io.Serializable;
import java.util.ArrayList;
/**
 * HomeActivity for drivers
 */
public class HomeActivity extends AppCompatActivity implements BottomNavigationView.OnNavigationItemSelectedListener {
    private String username;
    private Button upload;
    private Button finishService;
    private DatabaseReference mDatabase;
    private ArrayList<Object> servicesIds = new ArrayList<>();
    private ArrayList<Object> services = new ArrayList<>();
    private BottomNavigationView navigationView;

    /**
     * This method instance the activity
     */
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.home_activity);
        upload = findViewById(R.id.login);
        finishService = findViewById(R.id.button_final);
        navigationView = (BottomNavigationView) findViewById(R.id.navigationView);
        navigationView.setOnNavigationItemSelectedListener(HomeActivity.this);

        //GetUserName
        Intent intent = getIntent();
        username = intent.getStringExtra("id");
        Bundle args = intent.getBundleExtra("usernames");
        Bundle args2 = intent.getBundleExtra("data");
        services = (ArrayList<Object>) args.getSerializable("ARRAYLIST");
        servicesIds = (ArrayList<Object>) args2.getSerializable("ARRAYLIST_IDS");
        loadData();
        updateButtons(" Bem Vindo !");
    }

    /**
     * This method updates the state of buttons
     */
    private void updateButtons(String desc) {
        upload.setEnabled(false);
        upload.setBackgroundColor(Color.WHITE);
        finishService.setVisibility(View.INVISIBLE);
        upload.setText(desc);
        finishService.setText("");
    }

    /**
     * Method to control the menu
     */
    @Override
    public boolean onNavigationItemSelected(@NonNull MenuItem menuItem) {

        switch (menuItem.getItemId()) {
            case R.id.absence: {
                Intent i = new Intent(HomeActivity.this, AbsenceActivity.class);
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
                break;
            }
            case R.id.services: {
                Intent i = new Intent(HomeActivity.this, ServicesActivity.class);
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
     * This method load of services from Firebase
     */
    private void loadData() {
        mDatabase = FirebaseDatabase.getInstance().getReference();
        servicesIds = new ArrayList<>();
        services = new ArrayList<>();
        mDatabase.child("services").get().addOnCompleteListener(new OnCompleteListener<DataSnapshot>() {
            @Override
            public void onComplete(@NonNull Task<DataSnapshot> task) {
                for (DataSnapshot snapshot : task.getResult().getChildren()) {
                    String key = snapshot.getKey();
                    servicesIds.add(task.getResult().child(key).getKey());
                    services.add(task.getResult().child(key).getValue().toString());
                }

            }
        });

    }
}


