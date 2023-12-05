# Chat-WinForm

Welcome to Chat-WinForm, a straightforward client-server chat application with a user-friendly graphical interface based on WinForm.

## Overview

The project is designed with simplicity in mind and comprises two main components: the client and the server.

### Server
- The server component facilitates user connections via IP and port configurations.
- Upon connecting, users are prompted to set a unique username.
- Messages are transmitted using packets (refer to `packet.cs` file) and are broadcasted to all currently connected clients.

### Client
- Clients can connect to the server by specifying the server's IP address and port.
- Each client can set a personalized username upon joining the chat.
- The communication is handled through packets to ensure efficient message exchange.

## Getting Started

To run the chat application, follow these steps:

1. Clone the repository to your local machine.

   ```bash
   git clone git clone https://github.com/Gienkooo/Chatwinform
