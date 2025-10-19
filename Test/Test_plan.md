# COMPREHENSIVE TEST PLAN FOR TELEPHONE SWITCHING SYSTEM (TSS)

================================================================================

## 1. OVERVIEW

This test plan covers both black-box and white-box testing of the Phone Directory/Telephone Switching System application. Black-box tests focus on functional behavior from a user perspective, while white-box tests examine internal code logic, data structures, and execution paths.

================================================================================

## 2. TEST SCOPE

### IN SCOPE
- Phone entry file loading and validation
- Phone state transitions
- 2-way and 3-way call functionality
- Call transfer operations
- Conference call creation
- Command processing
- Error handling and validation
- User interface interactions
- Internal data structure integrity
- Code path coverage
- Boundary conditions

### OUT OF SCOPE
- Performance/load testing
- Multi-threading/concurrency
- Network/distributed operations
- Security testing

================================================================================

# PART A: BLACK-BOX TESTING

================================================================================

## 3. BLACK-BOX FUNCTIONAL TESTS
(Testing from user perspective without knowledge of internal implementation)

================================================================================

### 3.1 PHONE FILE LOADING TESTS (BLACK-BOX)

--------------------------------------------------------------------------------

#### 3.1.1 Valid File Loading

TEST ID: BB-FL-001
Description: Load valid phone entries
Input: Valid phones.txt with 5 entries
Expected Output: "Successfully loaded 5 phone entries."
Test Type: Positive

TEST ID: BB-FL-002
Description: Load file with comments
Input: File with # comments
Expected Output: Comments ignored, success message
Test Type: Positive

TEST ID: BB-FL-003
Description: Load file with empty lines
Input: File with blank lines
Expected Output: Empty lines ignored, success message
Test Type: Positive

TEST ID: BB-FL-004
Description: Load maximum entries
Input: File with exactly 20 entries
Expected Output: All 20 loaded successfully
Test Type: Boundary

--------------------------------------------------------------------------------

#### 3.1.2 Invalid File Handling

TEST ID: BB-FL-101
Description: Missing file
Input: Non-existent file path
Expected Output: "Error: phones.txt not found"
Test Type: Negative

TEST ID: BB-FL-102
Description: Empty file
Input: File with no valid entries
Expected Output: "Error: No valid phone entries found"
Test Type: Negative

TEST ID: BB-FL-103
Description: Phone number too short
Input: "1234 Alice"
Expected Output: Warning message, entry skipped
Test Type: Negative

TEST ID: BB-FL-104
Description: Phone number too long
Input: "123456 Alice"
Expected Output: Warning message, entry skipped
Test Type: Negative

TEST ID: BB-FL-105
Description: Non-numeric phone
Input: "12A45 Alice"
Expected Output: Warning message, entry skipped
Test Type: Negative

TEST ID: BB-FL-106
Description: Name too long
Input: "12345 VeryLongNameHere"
Expected Output: Warning message, entry skipped
Test Type: Negative

TEST ID: BB-FL-107
Description: Name with numbers
Input: "12345 Alice123"
Expected Output: Warning message, entry skipped
Test Type: Negative

TEST ID: BB-FL-108
Description: Single field only
Input: "12345"
Expected Output: "Unable to read line: incorrect number of fields"
Test Type: Negative

TEST ID: BB-FL-109
Description: Duplicate phone number
Input: Two entries with "12345"
Expected Output: Warning, first occurrence used
Test Type: Negative

TEST ID: BB-FL-110
Description: Duplicate name (different case)
Input: "Alice" and "ALICE"
Expected Output: Warning, first occurrence used
Test Type: Negative

TEST ID: BB-FL-111
Description: Exceed max entries
Input: 25 entries when max is 20
Expected Output: First 20 loaded, "Warning: additional lines skipped"
Test Type: Boundary

================================================================================

### 3.2 BASIC COMMAND OPERATIONS (BLACK-BOX)

--------------------------------------------------------------------------------

#### 3.2.1 Offhook Command

TEST ID: BB-OH-001
Description: Take phone offhook by name
Initial State: Alice ONHOOK
Command: offhook Alice
Expected Output: "Alice is now offhook (dialtone)."
Test Type: Positive

TEST ID: BB-OH-002
Description: Take phone offhook by number
Initial State: 12345 ONHOOK
Command: offhook 12345
Expected Output: "Alice is now offhook (dialtone)."
Test Type: Positive

TEST ID: BB-OH-003
Description: Case-insensitive name
Initial State: Alice ONHOOK
Command: offhook alice
Expected Output: Success message
Test Type: Positive

TEST ID: BB-OH-004
Description: Already offhook
Initial State: Alice OFFHOOK
Command: offhook Alice
Expected Output: "Alice is already offhook."
Test Type: Negative

TEST ID: BB-OH-005
Description: Non-existent phone
Initial State: N/A
Command: offhook 99999
Expected Output: "denial"
Test Type: Negative

TEST ID: BB-OH-006
Description: Non-existent name
Initial State: N/A
Command: offhook Unknown
Expected Output: "denial"
Test Type: Negative

TEST ID: BB-OH-007
Description: Missing parameter
Initial State: N/A
Command: offhook
Expected Output: "Invalid command syntax."
Test Type: Negative

--------------------------------------------------------------------------------

#### 3.2.2 Onhook Command

TEST ID: BB-ON-001
Description: Put phone onhook
Initial State: Alice OFFHOOK
Command: onhook Alice
Expected Output: "Alice is now onhook."
Test Type: Positive

TEST ID: BB-ON-002
Description: Already onhook
Initial State: Alice ONHOOK
Command: onhook Alice
Expected Output: "Alice is already onhook."
Test Type: Negative

TEST ID: BB-ON-003
Description: Hangup from 2-way call
Initial State: Alice-Bob TALKING
Command: onhook Alice
Expected Output: "Alice hung up." + "Bob hears silence."
Test Type: Positive

TEST ID: BB-ON-004
Description: Hangup from 3-way call
Initial State: Alice-Bob-Carol TALKING
Command: onhook Alice
Expected Output: "Alice hung up.", Bob-Carol continue
Test Type: Positive

TEST ID: BB-ON-005
Description: Non-existent phone
Initial State: N/A
Command: onhook 99999
Expected Output: "denial"
Test Type: Negative

================================================================================

### 3.3 CALL ESTABLISHMENT (BLACK-BOX)

--------------------------------------------------------------------------------

#### 3.3.1 Successful Calls

TEST ID: BB-CALL-001
Description: Basic call by name
Preconditions: Alice & Bob OFFHOOK
Command: call Bob
Expected Output: "Alice is now in a call with Bob."
Test Type: Positive

TEST ID: BB-CALL-002
Description: Call by phone number
Preconditions: Alice & Bob OFFHOOK
Command: call 23456
Expected Output: "Alice is now in a call with Bob."
Test Type: Positive

TEST ID: BB-CALL-003
Description: Case insensitive target
Preconditions: Alice & Bob OFFHOOK
Command: call bob
Expected Output: Call established
Test Type: Positive

--------------------------------------------------------------------------------

#### 3.3.2 Failed Calls

TEST ID: BB-CALL-101
Description: No caller offhook
Preconditions: All phones ONHOOK
Command: call Bob
Expected Output: "silence"
Test Type: Negative

TEST ID: BB-CALL-102
Description: Target doesn't exist
Preconditions: Alice OFFHOOK
Command: call Unknown
Expected Output: "denial"
Test Type: Negative

TEST ID: BB-CALL-103
Description: Self-call
Preconditions: Alice OFFHOOK
Command: call Alice
Expected Output: "denial"
Test Type: Negative

TEST ID: BB-CALL-104
Description: Target in another call
Preconditions: Alice OFFHOOK, Bob-Carol talking
Command: call Bob
Expected Output: "busy"
Test Type: Negative

TEST ID: BB-CALL-105
Description: Caller in call
Preconditions: Alice-Bob talking, Carol OFFHOOK
Command: call Carol
Expected Output: "busy"
Test Type: Negative

TEST ID: BB-CALL-106
Description: Target ONHOOK
Preconditions: Alice OFFHOOK, Bob ONHOOK
Command: call Bob
Expected Output: "silence"
Test Type: Negative

================================================================================

### 3.4 CONFERENCE CALLS (BLACK-BOX)

--------------------------------------------------------------------------------

#### 3.4.1 Successful Conference

TEST ID: BB-CONF-001
Description: Add 3rd to 2-way call
Preconditions: Alice-Bob talking, Carol OFFHOOK
Command: conference Carol
Expected Output: "Carol joined the conference."
Test Type: Positive

TEST ID: BB-CONF-002
Description: Conference by phone
Preconditions: Alice-Bob talking, Carol OFFHOOK
Command: conference 34567
Expected Output: "Carol joined the conference."
Test Type: Positive

--------------------------------------------------------------------------------

#### 3.4.2 Failed Conference

TEST ID: BB-CONF-101
Description: No active call
Preconditions: No one in call
Command: conference Carol
Expected Output: "silence"
Test Type: Negative

TEST ID: BB-CONF-102
Description: Target doesn't exist
Preconditions: Alice-Bob talking
Command: conference Unknown
Expected Output: "denial"
Test Type: Negative

TEST ID: BB-CONF-103
Description: Self-conference
Preconditions: Alice in call
Command: conference Alice
Expected Output: "denial"
Test Type: Negative

TEST ID: BB-CONF-104
Description: Target in another call
Preconditions: Alice-Bob talking, Carol-Dave talking
Command: conference Carol
Expected Output: "busy"
Test Type: Negative

TEST ID: BB-CONF-105
Description: Target ONHOOK
Preconditions: Alice-Bob talking, Carol ONHOOK
Command: conference Carol
Expected Output: "silence"
Test Type: Negative

TEST ID: BB-CONF-106
Description: Conference full (3 people)
Preconditions: Alice-Bob-Carol talking
Command: conference Dave
Expected Output: "busy (conference full)."
Test Type: Negative

================================================================================

### 3.5 CALL TRANSFER (BLACK-BOX)

--------------------------------------------------------------------------------

#### 3.5.1 Successful Transfer

TEST ID: BB-XFER-001
Description: Transfer with acceptance
Preconditions: Alice-Bob talking, Carol ONHOOK
Command & Input: transfer Carol, transferer: Alice
Answer: y
Expected Output: Carol joined, Alice hears silence
Test Type: Positive

TEST ID: BB-XFER-002
Description: Transfer declined
Preconditions: Alice-Bob talking, Carol ONHOOK
Command & Input: transfer Carol, transferer: Alice
Answer: n
Expected Output: Original call restored
Test Type: Positive

--------------------------------------------------------------------------------

#### 3.5.2 Failed Transfer

TEST ID: BB-XFER-101
Description: Target doesn't exist
Preconditions: Alice-Bob talking
Command: transfer Unknown
Expected Output: "denial"
Test Type: Negative

TEST ID: BB-XFER-102
Description: Transferer doesn't exist
Preconditions: Alice-Bob talking
Command & Input: transfer Carol, transferer: Unknown
Expected Output: "denial"
Test Type: Negative

TEST ID: BB-XFER-103
Description: Transfer to self
Preconditions: Alice-Bob talking
Command & Input: transfer Alice, transferer: Alice
Expected Output: "denial"
Test Type: Negative

TEST ID: BB-XFER-104
Description: Transferer not in call
Preconditions: Alice-Bob talking
Command & Input: transfer Carol, transferer: Dave
Expected Output: "denial"
Test Type: Negative

TEST ID: BB-XFER-105
Description: From 3-way call
Preconditions: Alice-Bob-Carol talking
Command & Input: transfer Dave, transferer: Alice
Expected Output: "denial"
Test Type: Negative

TEST ID: BB-XFER-106
Description: Target in another call
Preconditions: Alice-Bob talking, Carol-Dave talking
Command & Input: transfer Carol, transferer: Alice
Expected Output: "denial"
Test Type: Negative

TEST ID: BB-XFER-107
Description: Target not ONHOOK
Preconditions: Alice-Bob talking, Carol OFFHOOK
Command & Input: transfer Carol, transferer: Alice
Expected Output: "denial"
Test Type: Negative

TEST ID: BB-XFER-108
Description: Target in same call
Preconditions: Alice-Bob talking
Command & Input: transfer Bob, transferer: Alice
Expected Output: "already part of that call"
Test Type: Negative

================================================================================

### 3.6 SYSTEM COMMANDS (BLACK-BOX)

TEST ID: BB-SYS-001
Description: Display help
Command: help
Expected Result: Help menu displayed
Test Type: Positive

TEST ID: BB-SYS-002
Description: Display help by number
Command: 7
Expected Result: Help menu displayed
Test Type: Positive

TEST ID: BB-SYS-003
Description: Display status
Command: status
Expected Result: All phones and states shown
Test Type: Positive

TEST ID: BB-SYS-004
Description: Display status by number
Command: 6
Expected Result: All phones and states shown
Test Type: Positive

TEST ID: BB-SYS-005
Description: Exit by quit
Command: quit
Expected Result: Command count displayed, program exits
Test Type: Positive

TEST ID: BB-SYS-006
Description: Exit by exit
Command: exit
Expected Result: Command count displayed, program exits
Test Type: Positive

TEST ID: BB-SYS-007
Description: Exit by number
Command: 8
Expected Result: Command count displayed, program exits
Test Type: Positive

TEST ID: BB-SYS-008
Description: Invalid command
Command: invalid
Expected Result: "Invalid command."
Test Type: Negative

================================================================================

### 3.7 INPUT HANDLING (BLACK-BOX)

TEST ID: BB-INPUT-001
Description: Leading whitespace
Input: "  offhook Alice"
Expected Behavior: Trimmed, processed normally
Test Type: Boundary

TEST ID: BB-INPUT-002
Description: Trailing whitespace
Input: "offhook Alice  "
Expected Behavior: Trimmed, processed normally
Test Type: Boundary

TEST ID: BB-INPUT-003
Description: Multiple spaces
Input: "offhook    Alice"
Expected Behavior: Processed normally
Test Type: Boundary

TEST ID: BB-INPUT-004
Description: Exactly 50 characters
Input: 50-char command
Expected Behavior: Processed normally
Test Type: Boundary

TEST ID: BB-INPUT-005
Description: 51 characters
Input: 51-char command
Expected Behavior: Truncated to 50, warning shown
Test Type: Boundary

TEST ID: BB-INPUT-006
Description: Empty input
Input: ""
Expected Behavior: Ignored, prompt again
Test Type: Boundary

TEST ID: BB-INPUT-007
Description: Whitespace only
Input: "   "
Expected Behavior: Ignored, prompt again
Test Type: Boundary

TEST ID: BB-INPUT-008
Description: Case insensitive commands
Input: "OFFHOOK Alice"
Expected Behavior: Processed normally
Test Type: Positive

================================================================================

### 3.8 END-TO-END SCENARIOS (BLACK-BOX)

TEST ID: BB-E2E-001
Description: Complete call lifecycle
Test Steps:
    1. Alice offhook
    2. Bob offhook
    3. call Bob
    4. onhook Alice
Expected Results: Each step succeeds with correct messages
Test Type: Integration

TEST ID: BB-E2E-002
Description: Build 3-way conference
Test Steps:
    1. Alice & Bob offhook
    2. call Bob
    3. Carol offhook
    4. conference Carol
Expected Results: 3-way call established
Test Type: Integration

TEST ID: BB-E2E-003
Description: Conference then dropout
Test Steps:
    1. Alice-Bob-Carol in 3-way
    2. onhook Bob
Expected Results: Alice-Carol continue in 2-way
Test Type: Integration

TEST ID: BB-E2E-004
Description: Transfer workflow
Test Steps:
    1. Alice-Bob call
    2. transfer Carol (Alice transfers, Carol accepts)
Expected Results: Bob-Carol now talking
Test Type: Integration

TEST ID: BB-E2E-005
Description: Multiple simultaneous calls
Test Steps:
    1. Alice-Bob call
    2. Carol-Dave call
    3. status
Expected Results: Both calls shown independently
Test Type: Integration

================================================================================

# PART B: WHITE-BOX TESTING

================================================================================

## 4. WHITE-BOX STRUCTURAL TESTS
(Testing based on internal code structure, logic paths, and implementation)

================================================================================

### 4.1 PHONEFILELOADER CLASS (WHITE-BOX)

--------------------------------------------------------------------------------

#### 4.1.1 Code Path Coverage

TEST ID: WB-FL-001
Description: File not found path
Target Method: LoadPhoneEntries
Execution Path: File.Exists returns false
Input: Invalid path
Expected Internal State: Returns null, usedPhoneNumbers empty
Test Type: Path Coverage

TEST ID: WB-FL-002
Description: Empty usedPhoneNumbers set
Target Method: LoadPhoneEntries
Execution Path: No valid entries processed
Input: Empty file
Expected Internal State: usedPhoneNumbers.Count == 0
Test Type: Path Coverage

TEST ID: WB-FL-003
Description: Comment line skip
Target Method: ProcessLine
Execution Path: IsEmptyOrComment returns true
Input: "# comment"
Expected Internal State: Returns null, sets not modified
Test Type: Branch Coverage

TEST ID: WB-FL-004
Description: Empty line skip
Target Method: ProcessLine
Execution Path: IsEmptyOrComment returns true
Input: ""
Expected Internal State: Returns null, sets not modified
Test Type: Branch Coverage

TEST ID: WB-FL-005
Description: Split produces 1 part
Target Method: ProcessLine
Execution Path: parts.Length != 2 (less than)
Input: "12345"
Expected Internal State: Error message, returns null
Test Type: Branch Coverage

TEST ID: WB-FL-006
Description: Split produces 3 parts
Target Method: ProcessLine
Execution Path: parts.Length != 2 (more than)
Input: "12345 Alice Bob"
Expected Internal State: Error message, returns null
Test Type: Branch Coverage

TEST ID: WB-FL-007
Description: Phone validation failure
Target Method: ProcessLine
Execution Path: IsValidPhoneNumber returns false
Input: "1234 Alice"
Expected Internal State: Returns null, sets not modified
Test Type: Branch Coverage

TEST ID: WB-FL-008
Description: Name validation failure
Target Method: ProcessLine
Execution Path: IsValidName returns false
Input: "12345 A"
Expected Internal State: Returns null, sets not modified
Test Type: Branch Coverage

TEST ID: WB-FL-009
Description: Duplicate phone detected
Target Method: ProcessLine
Execution Path: IsDuplicatePhoneNumber returns true
Input: "12345 Bob" (after "12345 Alice")
Expected Internal State: Returns null, first entry kept
Test Type: Branch Coverage

TEST ID: WB-FL-010
Description: Duplicate name detected
Target Method: ProcessLine
Execution Path: IsDuplicateName returns true
Input: "23456 Alice" (after "12345 Alice")
Expected Internal State: Returns null, first entry kept
Test Type: Branch Coverage

TEST ID: WB-FL-011
Description: All validations pass
Target Method: ProcessLine
Execution Path: All validators return true/false correctly
Input: "12345 Alice"
Expected Internal State: Entry returned, both sets updated
Test Type: Path Coverage

TEST ID: WB-FL-012
Description: Max entries boundary
Target Method: LoadPhoneEntries
Execution Path: validEntriesLoaded >= maxEntries
Input: 20 valid + 5 more
Expected Internal State: First 20 loaded, warning shown once
Test Type: Loop Coverage

--------------------------------------------------------------------------------

#### 4.1.2 Boundary Value Analysis

TEST ID: WB-FL-101
Description: Phone length = 0
Method: IsValidPhoneNumber
Test Value: ""
Expected Result: false
Test Type: Boundary

TEST ID: WB-FL-102
Description: Phone length = 1
Method: IsValidPhoneNumber
Test Value: "1"
Expected Result: false
Test Type: Boundary

TEST ID: WB-FL-103
Description: Phone length = 4
Method: IsValidPhoneNumber
Test Value: "1234"
Expected Result: false
Test Type: Boundary

TEST ID: WB-FL-104
Description: Phone length = 5
Method: IsValidPhoneNumber
Test Value: "12345"
Expected Result: true
Test Type: Boundary

TEST ID: WB-FL-105
Description: Phone length = 6
Method: IsValidPhoneNumber
Test Value: "123456"
Expected Result: false
Test Type: Boundary

TEST ID: WB-FL-106
Description: Name length = 0
Method: IsValidName
Test Value: ""
Expected Result: false
Test Type: Boundary

TEST ID: WB-FL-107
Description: Name length = 1
Method: IsValidName
Test Value: "A"
Expected Result: true
Test Type: Boundary

TEST ID: WB-FL-108
Description: Name length = 12
Method: IsValidName
Test Value: "AliceAndBobs"
Expected Result: true
Test Type: Boundary

TEST ID: WB-FL-109
Description: Name length = 13
Method: IsValidName
Test Value: "AliceAndBobsy"
Expected Result: false
Test Type: Boundary

TEST ID: WB-FL-110
Description: Max entries = 20
Method: LoadPhoneEntries
Test Value: 20 entries
Expected Result: All loaded, no warning
Test Type: Boundary

TEST ID: WB-FL-111
Description: Max entries = 21
Method: LoadPhoneEntries
Test Value: 21 entries
Expected Result: 20 loaded, warning shown
Test Type: Boundary

--------------------------------------------------------------------------------

#### 4.1.3 Data Structure Validation

TEST ID: WB-FL-201
Description: usedPhoneNumbers populated
Method: ProcessLine
Test Scenario: Process valid entry
Verification: Contains("12345") == true
Test Type: State Verification

TEST ID: WB-FL-202
Description: usedNames case-insensitive
Method: ProcessLine
Test Scenario: Add "Alice" and "ALICE"
Verification: usedNames.Contains("alice") for both
Test Type: State Verification

TEST ID: WB-FL-203
Description: HashSet prevents duplicates
Method: ProcessLine
Test Scenario: Add same phone twice
Verification: usedPhoneNumbers.Count == 1
Test Type: State Verification

TEST ID: WB-FL-204
Description: List order preserved
Method: LoadPhoneEntries
Test Scenario: Load entries
Verification: entries[0] is first valid line
Test Type: State Verification

================================================================================

### 4.2 PHONESYSTEM CLASS (WHITE-BOX)

--------------------------------------------------------------------------------

#### 4.2.1 State Management Paths

TEST ID: WB-PS-001
Description: Initialize phoneStates
Method: Constructor
Execution Path: foreach loop over phoneBook
Input: 5 entries
Expected Internal State: phoneStates.Count == 5, all ONHOOK
Test Type: Path Coverage

TEST ID: WB-PS-002
Description: SetPhoneState - exists
Method: SetPhoneState
Execution Path: ContainsKey returns true
Input: Valid phone
Expected Internal State: phoneStates[phone] updated
Test Type: Branch Coverage

TEST ID: WB-PS-003
Description: SetPhoneState - not exists
Method: SetPhoneState
Execution Path: ContainsKey returns false
Input: Invalid phone
Expected Internal State: phoneStates unchanged
Test Type: Branch Coverage

TEST ID: WB-PS-004
Description: GetPhoneState - exists
Method: GetPhoneState
Execution Path: TryGetValue returns true
Input: Valid phone
Expected Internal State: Returns stored state
Test Type: Branch Coverage

TEST ID: WB-PS-005
Description: GetPhoneState - not exists
Method: GetPhoneState
Execution Path: TryGetValue returns false
Input: Invalid phone
Expected Internal State: Returns ONHOOK default
Test Type: Branch Coverage

TEST ID: WB-PS-006
Description: FindEntry - by phone first
Method: FindEntry
Execution Path: First foreach matches
Input: "12345"
Expected Internal State: Returns entry, second loop not executed
Test Type: Path Coverage

TEST ID: WB-PS-007
Description: FindEntry - by name second
Method: FindEntry
Execution Path: Second foreach matches
Input: "Alice"
Expected Internal State: First loop completes, then returns from second
Test Type: Path Coverage

TEST ID: WB-PS-008
Description: FindEntry - not found
Method: FindEntry
Execution Path: Both loops complete
Input: "Unknown"
Expected Internal State: Returns null
Test Type: Path Coverage

TEST ID: WB-PS-009
Description: FindEntry - case insensitive
Method: FindEntry
Execution Path: Name comparison with StringComparison.OrdinalIgnoreCase
Input: "alice"
Expected Internal State: Matches "Alice"
Test Type: Branch Coverage

--------------------------------------------------------------------------------

#### 4.2.2 Call Management - activeCalls List

TEST ID: WB-PS-101
Description: StartCall creates HashSet
Method: StartCall
Execution Path: New HashSet added to list
Precondition: Empty activeCalls
Expected activeCalls State: activeCalls.Count == 1, call.Count == 2
Test Type: State Verification

TEST ID: WB-PS-102
Description: StartCall updates both states
Method: StartCall
Execution Path: SetPhoneState called twice
Precondition: Valid phones
Expected activeCalls State: Both phones TALKING_2WAY
Test Type: State Verification

TEST ID: WB-PS-103
Description: IsPhoneInCall - true path
Method: IsPhoneInCall
Execution Path: Any returns true
Precondition: Phone in call
Expected activeCalls State: Returns true
Test Type: Branch Coverage

TEST ID: WB-PS-104
Description: IsPhoneInCall - false path
Method: IsPhoneInCall
Execution Path: Any returns false
Precondition: Phone not in call
Expected activeCalls State: Returns false
Test Type: Branch Coverage

TEST ID: WB-PS-105
Description: GetCallForPhone - found
Method: GetCallForPhone
Execution Path: FirstOrDefault returns call
Precondition: Phone in call
Expected activeCalls State: Returns HashSet reference
Test Type: Branch Coverage

TEST ID: WB-PS-106
Description: GetCallForPhone - not found
Method: GetCallForPhone
Execution Path: FirstOrDefault returns null
Precondition: Phone not in call
Expected activeCalls State: Returns null
Test Type: Branch Coverage

TEST ID: WB-PS-107
Description: TryAddToCall - success
Method: TryAddToCall
Execution Path: call != null && call.Count < 3
Precondition: 2-way call exists
Expected activeCalls State: call.Count == 3, all TALKING_3WAY
Test Type: Path Coverage

TEST ID: WB-PS-108
Description: TryAddToCall - null call
Method: TryAddToCall
Execution Path: call == null
Precondition: Phone not in call
Expected activeCalls State: Returns false, no changes
Test Type: Branch Coverage

TEST ID: WB-PS-109
Description: TryAddToCall - call full
Method: TryAddToCall
Execution Path: call.Count >= 3
Precondition: 3-way call exists
Expected activeCalls State: Returns false, no changes
Test Type: Branch Coverage

--------------------------------------------------------------------------------

#### 4.2.3 LeaveCall Logic Paths

TEST ID: WB-PS-201
Description: LeaveCall - null call
Method: LeaveCall
Execution Path: call == null, early return
Precondition: Phone not in call
Expected State After: No changes, no console output
Test Type: Path Coverage

TEST ID: WB-PS-202
Description: LeaveCall - 1 remaining
Method: LeaveCall
Execution Path: call.Count == 1 after remove
Precondition: 2-way call
Expected State After: activeCalls.Count decreases, remaining OFFHOOK
Test Type: Path Coverage

TEST ID: WB-PS-203
Description: LeaveCall - 2 remaining
Method: LeaveCall
Execution Path: call.Count == 2 after remove
Precondition: 3-way call
Expected State After: Both remaining TALKING_2WAY
Test Type: Path Coverage

TEST ID: WB-PS-204
Description: LeaveCall - 0 remaining
Method: LeaveCall
Execution Path: call.Count == 0 after remove
Precondition: Edge case
Expected State After: activeCalls removes empty set
Test Type: Path Coverage

TEST ID: WB-PS-205
Description: LeaveCall removes from set
Method: LeaveCall
Execution Path: call.Remove called
Precondition: Phone in call
Expected State After: call.Contains(phone) == false
Test Type: State Verification

TEST ID: WB-PS-206
Description: LeaveCall updates state
Method: LeaveCall
Execution Path: SetPhoneState to ONHOOK
Precondition: Any call
Expected State After: phoneStates[phone] == ONHOOK
Test Type: State Verification

--------------------------------------------------------------------------------

#### 4.2.4 TryTransferCall Logic Paths

TEST ID: WB-PS-301
Description: Transfer - null call
Method: TryTransferCall
Execution Path: call == null
Precondition: Transferer not in call
Expected State: Returns false, no changes
Test Type: Branch Coverage

TEST ID: WB-PS-302
Description: Transfer - not 2-way
Method: TryTransferCall
Execution Path: call.Count != 2
Precondition: 3-way call
Expected State: Returns false, no changes
Test Type: Branch Coverage

TEST ID: WB-PS-303
Description: Transfer - target in call
Method: TryTransferCall
Execution Path: IsPhoneInCall(target) == true
Precondition: Target busy
Expected State: Returns false, no changes
Test Type: Branch Coverage

TEST ID: WB-PS-304
Description: Transfer - target not in phoneStates
Method: TryTransferCall
Execution Path: TryGetValue returns false
Precondition: Invalid target
Expected State: Returns false, no changes
Test Type: Branch Coverage

TEST ID: WB-PS-305
Description: Transfer - target not ONHOOK
Method: TryTransferCall
Execution Path: targetState != ONHOOK
Precondition: Target OFFHOOK
Expected State: Returns false, no changes
Test Type: Branch Coverage

TEST ID: WB-PS-306
Description: Transfer accepted - y path
Method: TryTransferCall
Execution Path: response == "y"
Precondition: Valid transfer
Expected State: Target in call, transferer out, returns true
Test Type: Path Coverage

TEST ID: WB-PS-307
Description: Transfer declined - else path
Method: TryTransferCall
Execution Path: response != "y"
Precondition: Valid transfer
Expected State: Original call restored, returns false
Test Type: Path Coverage

TEST ID: WB-PS-308
Description: Transfer updates other party
Method: TryTransferCall
Execution Path: call.First(p => p != transferer)
Precondition: Valid transfer
Expected State: Other party correctly identified
Test Type: Logic Verification

TEST ID: WB-PS-309
Description: Transfer removes transferer
Method: TryTransferCall
Execution Path: call.Remove(transferer)
Precondition: During transfer
Expected State: call.Contains(transferer) == false
Test Type: State Verification

================================================================================

### 4.3 COMMANDPROCESSOR CLASS (WHITE-BOX)

--------------------------------------------------------------------------------

#### 4.3.1 HandleOffhook Paths

TEST ID: WB-CP-001
Description: Offhook - entry null
Method: HandleOffhook
Execution Path: entry == null
Precondition: Invalid identifier
Expected Flow: Prints "denial", returns early
Test Type: Path Coverage

TEST ID: WB-CP-002
Description: Offhook - not ONHOOK
Method: HandleOffhook
Execution Path: state != ONHOOK
Precondition: Phone already offhook
Expected Flow: Prints "already offhook", returns
Test Type: Path Coverage

TEST ID: WB-CP-003
Description: Offhook - success
Method: HandleOffhook
Execution Path: Both checks pass
Precondition: Valid ONHOOK phone
Expected Flow: Sets OFFHOOK_DIALTONE, prints success
Test Type: Path Coverage

--------------------------------------------------------------------------------

#### 4.3.2 HandleOnhook Paths

TEST ID: WB-CP-101
Description: Onhook - entry null
Method: HandleOnhook
Execution Path: entry == null
Precondition: Invalid identifier
Expected Flow: Prints "denial", returns early
Test Type: Path Coverage

TEST ID: WB-CP-102
Description: Onhook - in call
Method: HandleOnhook
Execution Path: IsPhoneInCall == true
Precondition: Phone in active call
Expected Flow: Calls LeaveCall, prints "hung up"
Test Type: Branch Coverage

TEST ID: WB-CP-103
Description: Onhook - already onhook
Method: HandleOnhook
Execution Path: state == ONHOOK
Precondition: Phone already onhook
Expected Flow: Prints "already onhook"
Test Type: Branch Coverage

TEST ID: WB-CP-104
Description: Onhook - success
Method: HandleOnhook
Execution Path: All checks pass
Precondition: Phone offhook, not in call
Expected Flow: Sets ONHOOK, prints success
Test Type: Path Coverage

--------------------------------------------------------------------------------

#### 4.3.3 HandleCall Complex Logic

TEST ID: WB-CP-201
Description: Call - no caller offhook
Method: HandleCall
Execution Path: FirstOrDefault returns null
Precondition: No OFFHOOK_DIALTONE
Expected Flow: Prints "silence", returns
Test Type: Path Coverage

TEST ID: WB-CP-202
Description: Call - target null
Method: HandleCall
Execution Path: FindEntry returns null
Precondition: Invalid target
Expected Flow: Prints "denial", returns
Test Type: Path Coverage

TEST ID: WB-CP-203
Description: Call - self call
Method: HandleCall
Execution Path: caller.Phone == target.Phone
Precondition: Same phone
Expected Flow: Prints "denial", returns
Test Type: Branch Coverage

TEST ID: WB-CP-204
Description: Call - target in call
Method: HandleCall
Execution Path: IsPhoneInCall(target) == true
Precondition: Target busy
Expected Flow: Prints "busy", returns
Test Type: Branch Coverage

TEST ID: WB-CP-205
Description: Call - caller in call
Method: HandleCall
Execution Path: IsPhoneInCall(caller) == true
Precondition: Caller busy
Expected Flow: Prints "busy", returns
Test Type: Branch Coverage

TEST ID: WB-CP-206
Description: Call - target ONHOOK
Method: HandleCall
Execution Path: GetPhoneState == ONHOOK
Precondition: Target onhook
Expected Flow: Prints "silence", returns
Test Type: Branch Coverage

TEST ID: WB-CP-207
Description: Call - success
Method: HandleCall
Execution Path: All checks pass
Precondition: Valid call scenario
Expected Flow: Calls StartCall, prints success
Test Type: Path Coverage

--------------------------------------------------------------------------------

#### 4.3.4 HandleTransfer Complex Logic

TEST ID: WB-CP-301
Description: Transfer - target null
Method: HandleTransfer
Execution Path: FindEntry(identifier) == null
Precondition: Invalid target
Expected Flow: Prints "denial", returns
Test Type: Path Coverage

TEST ID: WB-CP-302
Description: Transfer - empty input
Method: HandleTransfer
Execution Path: IsNullOrWhiteSpace(input)
Precondition: User enters nothing
Expected Flow: Prints "denial", returns
Test Type: Branch Coverage

TEST ID: WB-CP-303
Description: Transfer - transferer null
Method: HandleTransfer
Execution Path: FindEntry(transferer) == null
Precondition: Invalid transferer
Expected Flow: Prints "denial", returns
Test Type: Path Coverage

TEST ID: WB-CP-304
Description: Transfer - self transfer
Method: HandleTransfer
Execution Path: transferer.Phone == target.Phone
Precondition: Same phone
Expected Flow: Prints "denial", returns
Test Type: Branch Coverage

TEST ID: WB-CP-305
Description: Transfer - not 2-way call
Method: HandleTransfer
Execution Path: call == null || call.Count != 2
Precondition: Not in 2-way
Expected Flow: Prints "denial", returns
Test Type: Branch Coverage

TEST ID: WB-CP-306
Description: Transfer - target in same call
Method: HandleTransfer
Execution Path: call.Contains(target.Phone)
Precondition: Target already present
Expected Flow: Prints "already part", returns
Test Type: Branch Coverage

TEST ID: WB-CP-307
Description: Transfer - TryTransfer false
Method: HandleTransfer
Execution Path: TryTransferCall returns false
Precondition: Transfer validation fails
Expected Flow: Prints "denial", returns
Test Type: Path Coverage

TEST ID: WB-CP-308
Description: Transfer - success
Method: HandleTransfer
Execution Path: All checks pass
Precondition: Valid scenario
Expected Flow: Calls TryTransferCall, completes
Test Type: Path Coverage

--------------------------------------------------------------------------------

#### 4.3.5 HandleConference Logic

TEST ID: WB-CP-401
Description: Conference - no requester
Method: HandleConference
Execution Path: FirstOrDefault == null
Precondition: No one in call
Expected Flow: Prints "silence", returns
Test Type: Path Coverage

TEST ID: WB-CP-402
Description: Conference - participant null
Method: HandleConference
Execution Path: FindEntry == null
Precondition: Invalid participant
Expected Flow: Prints "denial", returns
Test Type: Path Coverage

TEST ID: WB-CP-403
Description: Conference - self
Method: HandleConference
Execution Path: requester.Phone == participant.Phone
Precondition: Same phone
Expected Flow: Prints "denial", returns
Test Type: Branch Coverage

TEST ID: WB-CP-404
Description: Conference - participant in call
Method: HandleConference
Execution Path: IsPhoneInCall(participant)
Precondition: Participant busy
Expected Flow: Prints "busy", returns
Test Type: Branch Coverage

TEST ID: WB-CP-405
Description: Conference - participant ONHOOK
Method: HandleConference
Execution Path: GetPhoneState == ONHOOK
Precondition: Participant onhook
Expected Flow: Prints "silence", returns
Test Type: Branch Coverage

TEST ID: WB-CP-406
Description: Conference - TryAdd success
Method: HandleConference
Execution Path: TryAddToCall returns true
Precondition: Valid add
Expected Flow: Prints "joined", success
Test Type: Path Coverage

TEST ID: WB-CP-407
Description: Conference - TryAdd fails
Method: HandleConference
Execution Path: TryAddToCall returns false
Precondition: Conference full
Expected Flow: Prints "busy (full)", returns
Test Type: Path Coverage

================================================================================

### 4.4 USERINTERFACE CLASS (WHITE-BOX)

--------------------------------------------------------------------------------

#### 4.4.1 GetUserInput Logic

TEST ID: WB-UI-001
Description: Null input
Method: GetUserInput
Execution Path: ReadLine returns null
Input: Ctrl+D/EOF
Expected Internal Processing: Returns null
Test Type: Branch Coverage

TEST ID: WB-UI-002
Description: Input requires trim
Method: GetUserInput
Execution Path: Trim called
Input: "  text  "
Expected Internal Processing: Leading/trailing removed
Test Type: Logic Verification

TEST ID: WB-UI-003
Description: Length check > 50
Method: GetUserInput
Execution Path: Length > 50 branch
Input: 60 chars
Expected Internal Processing: Substring(0,50), warning printed
Test Type: Branch Coverage

TEST ID: WB-UI-004
Description: Length check <= 50
Method: GetUserInput
Execution Path: Length <= 50 branch
Input: 50 chars
Expected Internal Processing: No truncation, no warning
Test Type: Branch Coverage

TEST ID: WB-UI-005
Description: Empty after trim
Method: GetUserInput
Execution Path: IsNullOrWhiteSpace after trim
Input: "   "
Expected Internal Processing: Returns null
Test Type: Branch Coverage

TEST ID: WB-UI-006
Description: Valid after trim
Method: GetUserInput
Execution Path: Returns trimmed string
Input: "offhook Alice"
Expected Internal Processing: Returns trimmed value
Test Type: Path Coverage

--------------------------------------------------------------------------------

#### 4.4.2 PrintStatus Reflection Logic

TEST ID: WB-UI-101
Description: Reflection success
Method: PrintStatus
Execution Path: GetField returns field
Scenario: Normal call
Expected Internal Behavior: activeCalls retrieved via reflection
Test Type: Path Coverage

TEST ID: WB-UI-102
Description: Reflection failure
Method: PrintStatus
Execution Path: GetField returns null
Scenario: Field renamed
Expected Internal Behavior: Uses empty list, no crash
Test Type: Branch Coverage

TEST ID: WB-UI-103
Description: Call count = 0
Method: PrintStatus
Execution Path: call == null path
Scenario: Phone not in call
Expected Internal Behavior: Prints simple state
Test Type: Branch Coverage

TEST ID: WB-UI-104
Description: Call count = 2
Method: PrintStatus
Execution Path: call.Count == 2 path
Scenario: 2-way call
Expected Internal Behavior: Finds other, prints TALKING_2WAY
Test Type: Branch Coverage

TEST ID: WB-UI-105
Description: Call count = 3
Method: PrintStatus
Execution Path: call.Count == 3 path
Scenario: 3-way call
Expected Internal Behavior: Uses Where and Select, prints TALKING_3WAY
Test Type: Branch Coverage

TEST ID: WB-UI-106
Description: Other count (safety)
Method: PrintStatus
Execution Path: Else path
Scenario: Edge case
Expected Internal Behavior: Prints state.ToString()
Test Type: Branch Coverage

================================================================================

### 4.5 PROGRAM MAIN LOOP (WHITE-BOX)

--------------------------------------------------------------------------------

#### 4.5.1 Command Parsing and Routing

TEST ID: WB-MAIN-001
Description: GetUserInput null
Section: Main loop
Execution Path: input == null
Input: EOF
Expected Routing: Breaks loop
Test Type: Branch Coverage

TEST ID: WB-MAIN-002
Description: Command count increment
Section: Main loop
Execution Path: commandCount++
Input: Any command
Expected Routing: Counter increases
Test Type: Logic Verification

TEST ID: WB-MAIN-003
Description: Case conversion
Section: Switch prep
Execution Path: cmd = parts[0].ToLower()
Input: "OFFHOOK"
Expected Routing: Converted to "offhook"
Test Type: Logic Verification

TEST ID: WB-MAIN-004
Description: Help by name
Section: Switch case
Execution Path: case "help"
Input: "help"
Expected Routing: Calls PrintHelp
Test Type: Path Coverage

TEST ID: WB-MAIN-005
Description: Help by number
Section: Switch case
Execution Path: case "7"
Input: "7"
Expected Routing: Calls PrintHelp
Test Type: Path Coverage

TEST ID: WB-MAIN-006
Description: Quit returns
Section: Switch case
Execution Path: case "quit" return
Input: "quit"
Expected Routing: ShowExitMessage, exits
Test Type: Path Coverage

TEST ID: WB-MAIN-007
Description: Parts validation
Section: Switch case
Execution Path: parts.Length != 2
Input: "offhook"
Expected Routing: Prints "Invalid syntax"
Test Type: Branch Coverage

TEST ID: WB-MAIN-008
Description: Default case
Section: Switch default
Execution Path: No match
Input: "invalid"
Expected Routing: Prints "Invalid command"
Test Type: Path Coverage

================================================================================

### 4.6 INTEGRATION - INTERNAL STATE CONSISTENCY (WHITE-BOX)

--------------------------------------------------------------------------------

#### 4.6.1 State Synchronization Tests

TEST ID: WB-INT-001
Description: phoneStates and activeCalls sync
Operations: StartCall
Verification Point: After call start
Expected Consistency: Both in phoneStates and activeCalls
Test Type: State Consistency

TEST ID: WB-INT-002
Description: LeaveCall cleanup
Operations: Leave 2-way call
Verification Point: After leave
Expected Consistency: Removed from activeCalls, state updated
Test Type: State Consistency

TEST ID: WB-INT-003
Description: State transition validity
Operations: ONHOOK -> OFFHOOK -> TALKING
Verification Point: Each step
Expected Consistency: No invalid state transitions
Test Type: State Machine

TEST ID: WB-INT-004
Description: HashSet reference consistency
Operations: TryAddToCall
Verification Point: After add
Expected Consistency: Same HashSet in activeCalls and GetCallForPhone
Test Type: Reference Integrity

TEST ID: WB-INT-005
Description: activeCalls count
Operations: Multiple operations
Verification Point: After each op
Expected Consistency: Count matches actual calls
Test Type: State Verification

--------------------------------------------------------------------------------

#### 4.6.2 Data Structure Integrity

TEST ID: WB-INT-101
Description: HashSet uniqueness
Scenario: Add duplicate to call
Verification: Check Contains
Expected Result: No duplicates in HashSet
Test Type: Structure Validation

TEST ID: WB-INT-102
Description: Dictionary key existence
Scenario: SetPhoneState with unknown
Verification: Check ContainsKey
Expected Result: No exception, silent fail
Test Type: Exception Prevention

TEST ID: WB-INT-103
Description: List ordering
Scenario: Add multiple calls
Verification: Iterate activeCalls
Expected Result: Maintains insertion order
Test Type: Structure Validation

TEST ID: WB-INT-104
Description: Null safety
Scenario: FindEntry with null
Verification: Method call
Expected Result: No NullReferenceException
Test Type: Null Handling

================================================================================

### 4.7 CODE COVERAGE METRICS

--------------------------------------------------------------------------------

#### 4.7.1 Statement Coverage Targets

CLASS: PhoneFileLoader
TARGET COVERAGE: 95%
CRITICAL PATHS: All validation branches

CLASS: PhoneSystem
TARGET COVERAGE: 95%
CRITICAL PATHS: All call management paths

CLASS: CommandProcessor
TARGET COVERAGE: 90%
CRITICAL PATHS: All command handlers

CLASS: UserInterface
TARGET COVERAGE: 80%
CRITICAL PATHS: Input handling and display

CLASS: Program
TARGET COVERAGE: 75%
CRITICAL PATHS: Main loop and routing

--------------------------------------------------------------------------------

#### 4.7.2 Branch Coverage Targets

METHOD: ProcessLine
TOTAL BRANCHES: 12
MUST-COVER BRANCHES: All validation branches

METHOD: TryTransferCall
TOTAL BRANCHES: 8
MUST-COVER BRANCHES: All failure conditions

METHOD: HandleCall
TOTAL BRANCHES: 10
MUST-COVER BRANCHES: All denial/busy paths

METHOD: LeaveCall
TOTAL BRANCHES: 6
MUST-COVER BRANCHES: All count conditions

================================================================================

# PART C: TEST EXECUTION FRAMEWORK

================================================================================

## 5. TEST EXECUTION STRATEGY

### 5.1 MANUAL TESTING (BLACK-BOX)

PURPOSE: Validate user-facing functionality and workflows

APPROACH:
1. Execute all BB tests through console interface
2. Follow test scripts exactly as documented
3. Verify console output matches expected results
4. Document any deviations

TOOLS:
- Console application
- Test data files
- Test execution checklist

--------------------------------------------------------------------------------

### 5.2 AUTOMATED UNIT TESTING (WHITE-BOX)

PURPOSE: Validate internal logic and code paths

APPROACH:

Example test structure:

[TestClass]
public class PhoneFileLoaderTests
{
    [TestMethod]
    public void WB_FL_001_FileNotFound_ReturnsNull()
    {
        // Arrange
        var loader = new PhoneFileLoader();
        
        // Act
        var result = loader.LoadPhoneEntries("nonexistent.txt", 20);
        
        // Assert
        Assert.IsNull(result);
    }
    
    [TestMethod]
    public void WB_FL_105_PhoneLength_BoundaryAt5_ReturnsTrue()
    {
        // Use reflection to test private method
        // Verify exactly 5 digits passes validation
    }
}

FRAMEWORKS:
- MSTest or NUnit
- Moq for mocking Console I/O
- Code coverage tools (dotCover, Coverlet)

--------------------------------------------------------------------------------

### 5.3 INTEGRATION TESTING (MIXED)

PURPOSE: Validate component interactions

APPROACH:
- Test PhoneSystem + CommandProcessor integration
- Test file loading to system initialization
- Verify state consistency across components

--------------------------------------------------------------------------------

### 5.4 CODE COVERAGE ANALYSIS

TOOLS: Visual Studio Code Coverage or Coverlet

PROCESS:
1. Run all automated tests with coverage enabled
2. Generate coverage report
3. Identify uncovered branches
4. Add tests for uncovered paths
5. Target 90%+ coverage for critical classes

================================================================================

## 6. TEST DATA REQUIREMENTS

### 6.1 BLACK-BOX TEST DATA

FILE: valid_phones.txt
CONTENTS: 5 standard entries

FILE: max_phones.txt
CONTENTS: Exactly 20 entries

FILE: over_max_phones.txt
CONTENTS: 25 entries

FILE: invalid_formats.txt
CONTENTS: Various format violations

FILE: duplicates.txt
CONTENTS: Duplicate names and numbers

FILE: comments.txt
CONTENTS: File with # comments and empty lines

--------------------------------------------------------------------------------

### 6.2 WHITE-BOX TEST DATA

- Boundary value files (4,5,6 digit phones)
- Name length boundaries (1,12,13 characters)
- Files triggering specific code paths
- Malformed CSV structures

================================================================================

## 7. DEFECT CLASSIFICATION

### 7.1 BLACK-BOX DEFECTS

- Incorrect user-facing behavior
- Wrong error messages
- Missing validation
- UI inconsistencies

--------------------------------------------------------------------------------

### 7.2 WHITE-BOX DEFECTS

- Logic errors in conditions
- State inconsistencies
- Memory leaks (uncleaned activeCalls)
- Unreachable code
- Missing null checks

================================================================================

## 8. SUCCESS CRITERIA

### 8.1 BLACK-BOX SUCCESS

- All positive test cases pass
- All negative test cases produce correct errors
- No unexpected behaviors in workflows
- User experience is consistent

--------------------------------------------------------------------------------

### 8.2 WHITE-BOX SUCCESS

- 90%+ code coverage achieved
- All critical paths tested
- All boundary conditions verified
- No null reference exceptions
- State consistency maintained across operations

================================================================================

## 9. TEST REPORTING