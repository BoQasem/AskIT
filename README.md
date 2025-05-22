# AskIT 🤖

## 📌 What is AskIT?
AskIT is a simple bot made to help employees who often get asked the same questions by users.
Instead of answering the same things over and over, the bot can reply for them.

This saves time for employees, and by collecting questions, we can later improve the interface and make things easier to use.



---

## 🎬 Demo
A demo video is available in the file: **AskIT_demo.mp4** 

[Click here to view the demo video](https://github.com/user-attachments/assets/5076d92b-e39e-42d6-bf2e-b2336ab9ba96)

---

## 🚀 How to Run

### 1. Create the Database

- Open SQL Server.
- Create a database named: `WhatsAppDB`.
- Update your `appsettings.Development.json` with your local DB connection:
  ```
  "ConnectionStrings": { 
    "WhatsAppDB": "Data Source=(localdb)\<ServerName>;Initial Catalog=WhatsAppDB;Integrated Security=true" 
  }
  ```
- Replace `<ServerName>` with your actual SQL Server instance name.

---

### 2. Run the Project Globally using ngrok

- Download and install [ngrok](https://ngrok.com/).
- Setup authentication (you’ll need an ngrok account).
- Open CMD and run:
  ```
  ngrok http https://localhost:<PORT>
  ```
- Replace `<PORT>` with the port your app is running on (e.g., 8080).

- When ngrok runs, you’ll get a global HTTPS URL like:
  ```
  https://<random>.ngrok-free.app
  ```

---

### 3. Connect to Meta (WhatsApp Business)

#### 3.1 Setup API

- Choose a phone number for the bot (each token supports one number only).
- Create an API token on Meta.
- Add the token in your `appsettings.Development.json`:
  ```
  "MetaDeveloper": { 
    "APIToken": "<YourToken>" 
  }
  ```

#### 3.2 Configure Webhook

- In Meta settings:
  - Set the callback URL to:
    ```
    https://<your-ngrok-url>/api/whatsapp
    ```
  - Set **Verify Token** – this must match the one you define in your controller (e.g., `"bo qasem"`).
  - Enable the `messages` field in Webhook.
  - Click **Verify and Save**.




