# :truck:
# **Retruck** 
The project is a decision-support system designed for transport companies handling services proposed by their clients. The system allows clients to register their transport requests, which are analyzed automatically, reducing decision-making time and ensuring the most suitable truck is selected.

## **Project Architecture**
<img title="Logotipo" alt="Logo" src="https://github.com/hugosilva12/retruck/blob/main/Doc/Diagrama_Classes_e_Arquitetura/ArquiteturaView.JPG" width="500">

## **Installation Tutorial**
To set up the project, follow these steps:
1. Create a database named ReTruck and configure it using the Windows connection string for SQL Server.;
2. Insert the provided data: <br>
`use ReTruck;`<br>
`Insert into Organization(id,name,enable,addresses,vatin) values('0180adf7-84ee-4255-ad55-69b15bf13c06','Demo',1,'Amarante',0);`<br>
`Insert into [User](id,organizationid,username,name,email,password,role,photofilename,userState) values('59E1108A-8102-4883-475E-08DA6BFB5528','0180ADF7-84EE-4255-AD55-69B15BF13C06','root','root','none','$s2$16384$8$1$uY6V8H6gj5MAulay5XNHI1e6dVVWCxR/wXSSDbfNF6c=$u46ZyKSZpXuuCPwXk04Qqw5bvlwmZUTyP4CINaFX4dQ=',3,'https://localhost:7226/Photos/root.png',0);`<br>

3. Start the backend: Run dotnet build and then dotnet run.
4. Install dependencies and start the frontend: Run npm install and then npm start.
5. Install and run the Mobile App using Android Studio. Ensure JDK 11 is used for compatibility.
**Super-Admin Credentials:** *username*:  `root` *password*: `string`

## **Running Integration Tests**
Follow these steps:
1. Comment out lines 122, 123, and 124 in the Startup class.
2. Comment out lines 154 to 159 in the Startup class.
3. Execute the tests. <br>
The test database is created automatically.

## **GitLab**
Used for version control, task management, and user story definition.

# Author
Hugo Silva
