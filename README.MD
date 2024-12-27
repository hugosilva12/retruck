# :truck:
# **Retruck** 
The project is a decision-support system designed for transport companies handling services proposed by their clients. The system allows clients to register their transport requests, which are analyzed automatically, reducing decision-making time and ensuring the most suitable truck is selected.

## **Project Architecture**
<img title="Logotipo" alt="Logo" src="https://github.com/hugosilva12/retruck/blob/main/Doc/Diagrama_Classes_e_Arquitetura/ArquiteturaView.JPG" width="500">

## **Tutorial de Instalação**
Para iniciar o projeto devem ser seguidos os seguintes passos:  
1. Criar a base de dados com o nome `ReTruck` e usar a conexão windows connection string do SQL Server;
 [**&nbsp;Clique aqui para visualizar o ficheiro que possui a configuração da base de dados**](https://gitlab.estg.ipp.pt/8180378-retruck/retruck-lds/-/blob/master/BackEnd/WebApplication1/WebApplication1/appsettings.json)<br>
2. Correr o script para criar as tabelas [**&nbsp;Clique aqui para visualizar o ficheiro**](https://gitlab.estg.ipp.pt/8180378-retruck/retruck-lds/-/blob/master/BackEnd/Sql_Scripts/createDataBase.sql); 
3. Inserir dados disponibilizados em (https://gitlab.estg.ipp.pt/8180378-retruck/retruck-lds/-/tree/master/BackEnd/Sql_Scripts) (**Importante**); <br>
`use ReTruck;`<br>
`Insert into Organization(id,name,enable,addresses,vatin) values('0180adf7-84ee-4255-ad55-69b15bf13c06','Demo',1,'Amarante',0);`<br>
`Insert into [User](id,organizationid,username,name,email,password,role,photofilename,userState) values('59E1108A-8102-4883-475E-08DA6BFB5528','0180ADF7-84EE-4255-AD55-69B15BF13C06','root','root','none','$s2$16384$8$1$uY6V8H6gj5MAulay5XNHI1e6dVVWCxR/wXSSDbfNF6c=$u46ZyKSZpXuuCPwXk04Qqw5bvlwmZUTyP4CINaFX4dQ=',3,'https://localhost:7226/Photos/root.png',0);`<br>

4. Iniciar o backend: `dotnet build` e `dotnet run`
5. Instalar dependências e iniciar o frontend: `npm install` e de seguida `npm start` 
6. Instalar e correr a APP Mobile através do `Android STUDIO`, deve ser usado o `JDK 11` para o correto funcionamento da APP. <br><br>
**Dados de Acesso do Super-Admin:** *username*:  `root` *password*: `string`

## **Executar testes de Integração**
Seguir os seguintes passos:
1. Comentar linhas 122,123 e 124 da Classe Startup.
2. Comentar linhas 154 à 159 da Classe Startup.
3. Executar Testes. <br>
A base de dados dos testes é criada automaticamente.

## **GitLab**
 Usado para controlo de versões, definição de tarefas, definição de user stories.

# Autor
8180378-Hugo Silva
