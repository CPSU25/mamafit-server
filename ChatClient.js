// Sample JavaScript code for testing SignalR connection with JWT authentication

// 1. Make sure you have @microsoft/signalr package installed
// npm install @microsoft/signalr

import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

class ChatClient {
    constructor(accessToken) {
        this.accessToken = accessToken;
        this.connection = null;
    }

    async connect() {
        try {
            // Create connection with access token in query string
            this.connection = new HubConnectionBuilder()
                .withUrl("https://your-api-domain.com/chatHub", {
                    accessTokenFactory: () => this.accessToken,
                    // Alternative method - pass token in query string
                    // skipNegotiation: true,
                    // transport: HttpTransportType.WebSockets
                })
                .withAutomaticReconnect()
                .configureLogging(LogLevel.Information)
                .build();

            // Set up event handlers BEFORE starting connection
            this.setupEventHandlers();

            // Start the connection
            await this.connection.start();
            console.log("✅ SignalR Connected successfully");

            // Debug: Check what claims are available
            await this.debugClaims();

            return true;
        } catch (error) {
            console.error("❌ SignalR Connection failed:", error);
            return false;
        }
    }

    setupEventHandlers() {
        // Listen for debug claims response
        this.connection.on("DebugClaims", (data) => {
            console.log("🔍 Debug Claims:", data);
        });

        // Listen for received messages
        this.connection.on("ReceiveMessage", (message) => {
            console.log("📩 Received message:", message);
            // Handle displaying the message in your UI
        });

        // Listen for message history
        this.connection.on("MessageHistory", (data) => {
            console.log("📚 Message history:", data);
            // Handle displaying message history
        });

        // Listen for errors
        this.connection.on("Error", (error) => {
            console.error("❌ SignalR Error:", error);
        });

        // Listen for join confirmation
        this.connection.on("JoinedRoom", (roomId) => {
            console.log(`✅ Joined room: ${roomId}`);
        });

        // Listen for message sent confirmation
        this.connection.on("MessageSent", (data) => {
            console.log("✅ Message sent confirmation:", data);
        });
    }

    async debugClaims() {
        try {
            await this.connection.invoke("DebugUserClaims");
        } catch (error) {
            console.error("❌ Failed to debug claims:", error);
        }
    }

    async joinRoom(roomId) {
        try {
            await this.connection.invoke("JoinRoom", roomId);
            console.log(`📥 Join room request sent for: ${roomId}`);
        } catch (error) {
            console.error("❌ Failed to join room:", error);
        }
    }

    async loadMessages(roomId, pageSize = 20, page = 1) {
        try {
            await this.connection.invoke("LoadMessages", roomId, pageSize, page);
            console.log(`📋 Load messages request sent for room: ${roomId}`);
        } catch (error) {
            console.error("❌ Failed to load messages:", error);
        }
    }

    async sendMessage(roomId, message) {
        try {
            const messageDto = {
                SenderId: "", // Will be set by server from JWT token
                ChatRoomId: roomId,
                Message: message,
                Type: "TEXT" // or whatever message types you support
            };

            await this.connection.invoke("SendMessage", messageDto);
            console.log("📤 Message sent:", messageDto);
        } catch (error) {
            console.error("❌ Failed to send message:", error);
        }
    }

    async disconnect() {
        if (this.connection) {
            await this.connection.stop();
            console.log("🔌 SignalR Disconnected");
        }
    }
}

// Usage example:
/*
// 1. Get your JWT token from login response
const accessToken = "your-jwt-token-here";

// 2. Create chat client instance
const chatClient = new ChatClient(accessToken);

// 3. Connect to SignalR hub
const connected = await chatClient.connect();

if (connected) {
    // 4. Join a chat room
    await chatClient.joinRoom("room-id-here");
    
    // 5. Load message history
    await chatClient.loadMessages("room-id-here", 20, 1);
    
    // 6. Send a message
    await chatClient.sendMessage("room-id-here", "Hello World!");
}

// 7. Disconnect when done
// await chatClient.disconnect();
*/

export default ChatClient;
