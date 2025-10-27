# Telephone Switching System (TSS)

A command-line telephone switching simulation for managing phone calls and conferences.

## 📋 What is TSS?

TSS is a telephone switching simulator that lets you manage a virtual phone network. You can make calls between phones, create conference calls with up to 3 participants, transfer calls, and monitor the status of all phones in your network.

## 🚀 Getting Started

### System Requirements

- Windows, macOS, or Linux
- .NET 9.0 Runtime (if not included with the executable)

### Running the Application

1. Create a `phones.txt` file in the same directory as the executable (see format below)
2. Run the executable:
   - **Windows**: Double-click `PhoneDirectory.exe` or run from Command Prompt
   - **macOS/Linux**: Open Terminal and run `./PhoneDirectory`

## 📝 Setting Up Your Phone Directory

Before running TSS, you need to create a `phones.txt` file in the same folder as the executable.

### File Format

```
# Phone Directory
# Format: NNNNN Name
12345 Alice
23456 Bob
34567 Carol
45678 Dave
56789 Eve
```

### Rules

- **Phone Numbers**: Must be exactly 5 digits (can start with 0)
- **Names**: 1 to 12 letters only (no numbers or special characters)
- **Comments**: Lines starting with `#` are ignored
- **Blank Lines**: Empty lines are ignored
- **Maximum**: 20 phone entries
- **Format**: One space between phone number and name

### Example Valid Entries

```
12345 Alice      ✓
00001 Bob        ✓
98765 Z          ✓
11111 Verylongna ✓
```

### Example Invalid Entries

```
1234 Alice       ✗ (phone number too short)
123456 Bob       ✗ (phone number too long)
12A45 Carol      ✗ (letters in phone number)
12345 Alice123   ✗ (numbers in name)
12345 TooLongName ✗ (name over 12 characters)
12345            ✗ (missing name)
```

## 🎮 How to Use TSS

### Command Reference

Once the program is running, enter commands at the prompt. Commands are **case-insensitive**.

| Command | What It Does | Example |
|---------|-------------|---------|
| `offhook Alice` | Take phone off the hook (ready to dial) | `offhook 12345` |
| `onhook Bob` | Hang up the phone | `onhook Carol` |
| `call Carol` | Make a call to another phone | `call 34567` |
| `conference Dave` | Add a third person to your call | `conference Eve` |
| `transfer Eve` | Transfer your call to someone else | `transfer 56789` |
| `status` | Show all phones and their current status | `status` |
| `help` | Show list of commands | `help` |
| `quit` | Exit the program | `exit` |

**Tip**: You can use either the phone number or the person's name in commands!

### Quick Start Example

Here's a simple example to get you started:

```
Enter command: offhook Alice
Alice is now offhook (dialtone).

Enter command: offhook Bob
Bob is now offhook (dialtone).

Enter command: call Bob
Who is making the call?: Alice
Alice is now in a call with Bob.

Enter command: status

System Status:
-------------
12345 (Alice): TALKING_2WAY with 23456 (Bob)
23456 (Bob): TALKING_2WAY with 12345 (Alice)
34567 (Carol): ONHOOK
45678 (Dave): ONHOOK
56789 (Eve): ONHOOK

Active Calls: 1
    - Call 1: Alice <-> Bob

Enter command: onhook Alice
Alice hung up.
Bob hears silence.
```

## 📞 Common Scenarios

### Making a Basic Call

1. Take both phones off the hook:
   ```
   offhook Alice
   offhook Bob
   ```

2. Make the call:
   ```
   call Bob
   Who is making the call?: Alice
   ```

3. When finished, hang up:
   ```
   onhook Alice
   ```

### Creating a Conference Call (3-Way)

1. Start with two people talking:
   ```
   offhook Alice
   offhook Bob
   call Bob
   (enter Alice as caller)
   ```

2. Have a third person ready:
   ```
   offhook Carol
   ```

3. Add them to the call:
   ```
   conference Carol
   ```

Now Alice, Bob, and Carol are all talking together!

### Transferring a Call

1. While in a 2-way call (Alice and Bob talking):
   ```
   transfer Carol
   Who is transferring the call? Alice
   ```

2. Carol's phone rings:
   ```
   Carol is ringing...
   Answer call? (y/n): y
   ```

3. If Carol answers "y", Bob and Carol are now talking. Alice has hung up.

4. If Carol answers "n", the original call between Alice and Bob continues.

## 📊 Understanding Phone Status

When you run the `status` command, you'll see phones in different states:

- **ONHOOK**: Phone is hung up (not in use)
- **OFFHOOK_DIALTONE**: Phone is picked up and ready to dial
- **TALKING_2WAY**: Two phones are connected in a call
- **TALKING_3WAY**: Three phones are in a conference call

## ⚠️ Error Messages Explained

You might see these messages:

- **"denial"**: You tried something that's not allowed (e.g., calling yourself, calling a non-existent phone)
- **"busy"**: The phone you're trying to call is already in another call
- **"silence"**: You tried to call someone, but their phone is hung up (onhook)
- **"Invalid command"**: The command you entered doesn't exist (use `help` to see valid commands)
- **"Invalid command syntax"**: You didn't provide the required information (e.g., you typed `call` but didn't say who to call)

## 🚫 Important Limitations

- **Maximum 20 phones** in your directory
- **Maximum 3 people** per call (2-way or 3-way only)
- **One call per phone** - a phone can't be in two calls at once
- **Can't call yourself** - Alice can't call Alice
- **Transfer only works from 2-way calls** - you can't transfer from a 3-way conference
- **Conference only works from 2-way calls** - you can't add a 4th person

## 🐛 Troubleshooting

### "Error: phones.txt not found"

**Problem**: The program can't find your phone directory file.

**Solution**: Make sure you have a file named `phones.txt` (all lowercase) in the same folder as the executable.

### "Error: No valid phone entries found"

**Problem**: Your `phones.txt` file exists but has no valid entries.

**Solution**: Check your file format:
- Phone numbers must be exactly 5 digits
- Names must be 1-12 letters only
- There must be exactly one space between the phone number and name

### Commands Don't Work

**Problem**: You type a command but nothing happens or you get an error.

**Solution**: 
- Make sure you spell the command correctly (though case doesn't matter)
- Include the required information (e.g., `offhook Alice`, not just `offhook`)
- Type `help` to see the list of valid commands

### "denial" When Trying to Call

**Possible Causes**:
- The phone you're calling doesn't exist - check the exact spelling
- You're trying to call yourself
- The person making the call isn't off the hook
- The phone you're calling is on the hook (hung up)

**Solution**: 
1. Check `status` to see all phones
2. Make sure both phones are `offhook` before calling
3. Use the exact name or phone number from your `phones.txt` file

### "busy" When Trying to Call

**Problem**: The phone is already in another call.

**Solution**: Wait for them to finish their current call, or have them hang up first.

## 💡 Tips for Using TSS

1. **Use the `status` command often** to see what's happening with all the phones
2. **Names are case-insensitive** - you can type `alice`, `Alice`, or `ALICE`
3. **Phone numbers work too** - if you can't remember a name, use the 5-digit number
4. **Both people must be offhook** before you can start a call
5. **Check for typos** - if you get "denial", you might have misspelled a name
6. **Read the prompts** - some commands (like transfer) will ask you for additional information

## 🆘 Getting Help

While the program is running, type `help` to see the list of available commands.

To exit the program at any time, type `quit` or `exit`.

## 📄 Quick Reference Card

```
┌─────────────────────────────────────────────┐
│         TSS QUICK REFERENCE                 │
├─────────────────────────────────────────────┤
│ offhook <name>     - Pick up phone          │
│ onhook <name>      - Hang up phone          │
│ call <name>        - Call someone           │
│ conference <name>  - Add 3rd person to call │
│ transfer <name>    - Transfer your call     │
│ status             - Show all phone states  │
│ help               - Show help menu         │
│ quit               - Exit program           │
└─────────────────────────────────────────────┘

Remember: Both phones must be OFFHOOK before calling!
```

---

**Version**: TSS 1.0  
**Platform**: .NET 9.0  
**Type**: Console Application

For technical support or questions, refer to the documentation provided with your executable.
