package com.example.a8180378;

import android.Manifest;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.app.ActivityCompat;

import com.example.a8180378.models.Login;
import com.google.android.gms.tasks.OnCompleteListener;
import com.google.android.gms.tasks.Task;
import com.google.firebase.database.DataSnapshot;
import com.google.firebase.database.DatabaseReference;
import com.google.firebase.database.FirebaseDatabase;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.Serializable;
import java.util.ArrayList;

/**
 * LoginActivity for all users of app
 */
public class LoginActivity extends AppCompatActivity {

    private Button login;
    private String username;
    private EditText mUserName, password;
    private DatabaseReference mDatabase;
    private DatabaseReference mDatabase2;
    private ArrayList<Object> servicesIds = new ArrayList<>();
    private ArrayList<Object> services = new ArrayList<>();
    private ArrayList<com.example.a8180378.models.Login> listUsers = new ArrayList<>();
    private static final int REQUEST_FINE_LOCATION = 100;

    /**
     * This method instance the activity
     */
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        login = findViewById(R.id.login);

        //load services
        loadData();

        //load users for auth
        loadUsers();

        requestPermissions();

        mUserName = findViewById(R.id.pass_email);
        password = findViewById(R.id.pass_word);
        login.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (verifyLogin(mUserName.getText().toString().trim(), password.getText().toString().trim())) {
                    if (getRole(mUserName.getText().toString().trim()) == 0) { // driver
                        Intent i = new Intent(LoginActivity.this, HomeActivity.class);
                        username = mUserName.getText().toString().trim();
                        i.putExtra("id", username);
                        i.putExtra("data", servicesIds.toString());
                        Bundle args = new Bundle();
                        Bundle args_2 = new Bundle();
                        args.putSerializable("ARRAYLIST", (Serializable) services);
                        args_2.putSerializable("ARRAYLIST_IDS", (Serializable) servicesIds);
                        i.putExtra("usernames", args);
                        i.putExtra("data", args_2);
                        startActivity(i);
                    } else if (getRole(mUserName.getText().toString().trim()) == 2) {
                        Intent i = new Intent(LoginActivity.this, ClientActivity.class);
                        username = mUserName.getText().toString().trim();
                        i.putExtra("id", username);
                        i.putExtra("data", servicesIds.toString());
                        Bundle args = new Bundle();
                        Bundle args_2 = new Bundle();
                        args.putSerializable("ARRAYLIST", (Serializable) services);
                        args_2.putSerializable("ARRAYLIST_IDS", (Serializable) servicesIds);
                        i.putExtra("usernames", args);
                        i.putExtra("data", args_2);
                        startActivity(i);
                    }
                } else {
                    Toast.makeText(LoginActivity.this, "Dados Inválidos", Toast.LENGTH_SHORT).show();
                }
            }
        });

    }

    /**
     * This method load of services from Firebase
     */
    private void loadData() {
        mDatabase = FirebaseDatabase.getInstance().getReference();

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

    /**
     * This method load of users from Firebase
     */
    private void loadUsers() {
        mDatabase2 = FirebaseDatabase.getInstance().getReference();
        mDatabase2.child("users").get().addOnCompleteListener(new OnCompleteListener<DataSnapshot>() {
            @Override
            public void onComplete(@NonNull Task<DataSnapshot> task) {
                for (DataSnapshot snapshot : task.getResult().getChildren()) {
                    String key = snapshot.getKey();
                    try {
                        JSONObject jsonObject = new JSONObject(task.getResult().child(key).getValue().toString());
                        System.out.println(jsonObject.get("username").toString());
                        System.out.println(jsonObject.get("password").toString());
                        listUsers.add(new Login(jsonObject.get("username").toString(), jsonObject.get("password").toString(), Integer.valueOf(jsonObject.get("role").toString())));

                    } catch (JSONException e) {
                        e.printStackTrace();

                    }
                }
            }
        });

    }

    /**
     * This method asks for permission to access the cell phone location
     */
    private void requestPermissions() {
        ActivityCompat.requestPermissions(this, new String[]{Manifest.permission.ACCESS_FINE_LOCATION}, REQUEST_FINE_LOCATION);

    }

    /**
     * Checks the authentication data entered by the user
     * @param username the username of user
     * @param password the password of user
     * @return true if data is valid, false otherwise
     */
    public boolean verifyLogin(String username, String password) {
        if (listUsers.size() != 0) {
            for (Login login : listUsers) {
                if (login.getId().equals(username) && login.getPassword().equals(password)) {
                    return true;
                }
            }
        }

        return false;
    }

    /**
     * Get the role of user
     * @param username the username of user
     * @return the role of user, -1 if the username not exists in list
     */
    public int getRole(String username) {
        if (listUsers.size() != 0) {
            for (Login login : listUsers) {
                if (login.getId().equals(username)) {
                    return login.getRole();
                }
            }
        }
        return -1;
    }
}