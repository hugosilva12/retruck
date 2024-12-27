package com.example.a8180378;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;

import androidx.appcompat.app.AppCompatActivity;
/**
 * HomeActivity for clients
 */
public class ClientActivity extends AppCompatActivity {

    private Button registerTruck, history;

    private String username;

    /**
     * This method instance the activity
     */
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.customer_activity);
        registerTruck = findViewById(R.id.login);
        history = findViewById(R.id.historico);

        //GetUserName
        Intent intent = getIntent();
        username = intent.getStringExtra("id");
        history.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

            }
        });

        registerTruck.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent i = new Intent(ClientActivity.this, RegistrationTransportActivity.class);
                i.putExtra("id", username);
                Bundle args = new Bundle();
                Bundle args_2 = new Bundle();
                i.putExtra("usernames", args);
                i.putExtra("data", args_2);
                startActivity(i);
            }
        });
    }


}


