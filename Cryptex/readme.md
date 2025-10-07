# Cryptex CLI 🔐

A secure RSA-encrypted command-line messenger for encrypted communication.

## Features ✨

- 🔐 **RSA Encryption** with 2048-bit keys
- 💬 **Chat Session Management** with multiple contacts
- 📁 **Local File Storage** - no database required
- 🛡️ **High Security** using standard algorithms
- 🚀 **Easy to Use** command-line interface
- 🔧 **Configurable** storage path
- 📱 **Cross-Platform** works on Windows, macOS, Linux

## Installation 🚀

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later

### Setup
```bash
# Clone the repository
git clone <repository-url>
cd Cryptex

# Build the project
dotnet build
```

## Quick Start 🏃‍♂️
```bash
# 1. Create your account
dotnet run setup

# 2. View your public key
dotnet run my-info

# 3. Create a chat session
dotnet run create-session chat-with-friend

# 4. Set friend's public key
dotnet run set-friend-key chat-with-friend "-----BEGIN PUBLIC KEY-----..."

# 5. Encrypt a message
dotnet run encrypt chat-with-friend "Hello! This is a test message"

# 6. Decrypt a message
dotnet run decrypt chat-with-friend "base64-encrypted-message..."
```

## Complete Command Reference 📋
### Account Management 👤
```bash
# Create user account (first time)
dotnet run setup

# Show your public key and information
dotnet run my-info

# Change your username
dotnet run edit-profile

# Generate new RSA keys
dotnet run regenerate-keys

# Repair account issues
dotnet run repair-account

# Completely reset account (deletes all data)
dotnet run reset-account
```

## Session Management 💬
```bash
# Create new chat session
dotnet run create-session <session-name>

# Set friend's public key in session
dotnet run set-friend-key <session-name> <public-key-or-path>

# List all chat sessions
dotnet run list-sessions
```

## Encryption & Decryption 🔐
```bash
# Encrypt message for your friend
dotnet run encrypt <session-name> "<message>"

# Decrypt message from your friend
dotnet run decrypt <session-name> "<encrypted-message-base64>"
```

## Configuration ⚙️
```bash
# Show current configuration
dotnet run show-config

# Change storage location
dotnet run change-storage <new-path>

# Fix storage path issues
dotnet run fix-storage

# Show help
dotnet run help
```

## Chat Workflow 🔄
### Step 1: Initial Setup
1. #### Both users run setup to create accounts
2. #### Each user runs my-info and shares their public key

### Step 2: Create Chat Session
1. #### Each user creates a session:

```bash
dotnet run create-session chat-with-friend
```

2. #### Each user sets the other's public key:

```bash
dotnet run set-friend-key chat-with-friend "friend-public-key-here"
```

### Step 3: Exchange Messages
Sending: Messages are encrypted with friend's public key

Receiving: Messages are decrypted with your private key

## Storage Structure 📁
```text
~/.cryptex/
├── config.json         # Configuration file
└── users/
└── <username>/
├── private.pem         # Your private key (NEVER SHARE)
├── public.pem          # Your public key (share this)
└── sessions/
└── <session-name>/
├── my_public.pem       # Copy of your public key
├── other_public.pem    # Friend's public key
└── message_*.enc       # Encrypted messages
```

## Practical Example 💡
### User A (Alice) chatting with User B (Bob)
#### User A (Alice):

```bash
dotnet run setup
# Enter: alice

dotnet run my
```