# COMPREHENSIVE TEST PLAN FOR TELEPHONE SWITCHING SYSTEM (TSS)

================================================================================

## 1. OVERVIEW

This test plan covers both black-box and white-box testing of the Phone Directory/Telephone Switching System application. Black-box tests focus on functional behavior from a user perspective, while white-box tests examine internal code logic, data structures, and execution paths.
 
**Project**: CS 4230 - Telephone Switching System  

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
Priority: P0 (Critical)
Description: Load valid phone entries
Input: Valid phones.txt with 5 entries
Expected Output: "Successfully loaded 5 phone entries."
Test Type: Positive

TEST ID: BB-FL-002
Priority: P1 (High)
Description: Load file with comments
Input: File with # comments
Expected Output: Comments ignored, success message
Test Type: Positive

TEST ID: BB-FL-003
Priority: P1 (High)
Description: Load file with empty lines
Input: File with blank lines
Expected Output: Empty lines ignored, success message
Test Type: Positive

TEST ID: BB-FL-004
Priority: P0 (Critical)
Description: Load maximum entries
Input: File with exactly 20 entries
Expected Output: All 20 loaded successfully
Test Type: Boundary

--------------------------------------------------------------------------------

#### 3.1.2 Invalid File Handling

TEST ID: BB-FL-101
Priority: P0 (Critical)
Description: Missing file
Input: Non-existent file path
Expected Output: "Error: phones.txt not found"
Test Type: Negative

TEST ID: BB-FL-102
Priority: P0 (Critical)
Description: Empty file
Input: File with no valid entries
Expected Output: "Error: No valid phone entries found"
Test Type: Negative

TEST ID: BB-FL-103
Priority: P1 (High)
Description: Phone number too short
Input: "1234 Alice"
Expected Output: Warning message, entry skipped
Test Type: Negative

TEST ID: BB-FL-104
Priority: P1 (High)
Description: Phone number too long
Input: "123456 Alice"
Expected Output: Warning message, entry skipped
Test Type: Negative

TEST ID: BB-FL-105
Priority: P1 (High)
Description: Non-numeric phone
Input: "12A45 Alice"
Expected Output: Warning message, entry skipped
Test Type: Negative

TEST ID: BB-FL-106
Priority: P1 (High)
Description: Name too long
Input: "12345 VeryLongNameHere"
Expected Output: Warning message, entry skipped
Test Type: Negative

TEST ID: BB-FL-107
Priority: P1 (High)
Description: Name with numbers
Input: "12345 Alice123"
Expected Output: Warning message, entry skipped
Test Type: Negative

TEST ID: BB-FL-108
Priority: P1 (High)
Description: Single field only
Input: "12345"
Expected Output: "Unable to read line: incorrect number of fields"
Test Type: Negative

TEST ID: BB-FL-109
Priority: P1 (High)
Description: Duplicate phone number
Input: Two entries with "12345"
Expected Output: Warning, first occurrence used
Test Type: Negative

TEST ID: BB-FL-110
Priority: P1 (High)
Description: Duplicate name (different case)
Input: "Alice" and "ALICE"
Expected Output: Warning, first occurrence used
Test Type: Negative

TEST ID: BB-FL-111
Priority: P0 (Critical)
Description: Exceed max entries
Input: 25 entries when max is 20
Expected Output: First 20 loaded, "Warning: additional lines skipped"
Test Type: Boundary

================================================================================

### 3.2 BASIC COMMAND OPERATIONS (BLACK-BOX)

--------------------------------------------------------------------------------

#### 3.2.1 Offhook Command

TEST ID: BB-OH-001
Priority: P0 (Critical)
Description: Take phone offhook by name
Initial State: Alice ONHOOK
Command: offhook Alice
Expected Output: "Alice is now offhook (dialtone)."
Test Type: Positive

TEST ID: BB-OH-002
Priority: P0 (Critical)
Description: Take phone offhook by number
Initial State: 12345 ONHOOK
Command: offhook 12345
Expected Output: "Alice is now offhook (dialtone)."
Test Type: Positive

TEST ID: BB-OH-003
Priority: P1 (High)
Description: Case-insensitive name
Initial State: Alice ONHOOK
Command: offhook alice
Expected Output: Success message
Test Type: Positive

TEST ID: BB-OH-004
Priority: P1 (High)
Description: Already offhook
Initial State: Alice OFFHOOK
Command: offhook Alice
Expected Output: "Alice is already offhook."
Test Type: Negative

TEST ID: BB-OH-005
Priority: P0 (Critical)
Description: Non-existent phone
Initial State: N/A
Command: offhook 99999
Expected Output: "denial"
Test Type: Negative

TEST ID: BB-OH-006
Priority: P0 (Critical)
Description: Non-existent name
Initial State: N/A
Command: offhook Unknown
Expected Output: "denial"
Test Type: Negative

TEST ID: BB-OH-007
Priority: P1 (High)
Description: Missing parameter
Initial State: N/A
Command: offhook
Expected Output: "Invalid command syntax."
Test Type: Negative

--------------------------------------------------------------------------------

#### 3.2.2 Onhook Command

TEST ID: BB-ON-001
Priority: P0 (Critical)
Description: Put phone onhook
Initial State: Alice OFFHOOK
Command: onhook Alice
Expected Output: "Alice is now onhook."
Test Type: Positive

TEST ID: BB-ON-002
Priority: P1 (High)
Description: Already onhook
Initial State: Alice ONHOOK
Command: onhook Alice
Expected Output: "Alice is already onhook."
Test Type: Negative

TEST ID: BB-ON-003
Priority: P0 (Critical)
Description: Hangup from 2-way call
Initial State: Alice-Bob TALKING
Command: onhook Alice
Expected Output: "Alice hung up." + "Bob hears silence."
Test Type: Positive

TEST ID: BB-ON-004
Priority: P0 (Critical)
Description: Hangup from 3-way call
Initial State: Alice-Bob-Carol TALKING
Command: onhook Alice
Expected Output: "Alice hung up.", Bob-Carol continue
Test Type: Positive

TEST ID: BB-ON-005
Priority: P0 (Critical)
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
Priority: P0 (Critical)
Description: Basic call by name
Preconditions: Alice & Bob OFFHOOK
Command: call Bob
Expected Output: "Alice is now in a call with Bob."
Test Type: Positive

TEST ID: BB-CALL-002
Priority: P0 (Critical)
Description: Call by phone number
Preconditions: Alice & Bob OFFHOOK
Command: call 23456
Expected Output: "Alice is now in a call with Bob."
Test Type: Positive

TEST ID: BB-CALL-003
Priority: P1 (High)
Description: Case insensitive target
Preconditions: Alice & Bob OFFHOOK
Command: call bob
Expected Output: Call established
Test Type: Positive

--------------------------------------------------------------------------------

#### 3.3.2 Failed Calls

TEST ID: BB-CALL-101
Priority: P0 (Critical)
Description: No caller offhook
Preconditions: All phones ONHOOK
Command: call Bob
Expected Output: "silence"
Test Type: Negative

TEST ID: BB-CALL-102
Priority: P0 (Critical)
Description: Target doesn't exist
Preconditions: Alice OFFHOOK
Command: call Unknown
Expected Output: "denial"
Test Type: Negative

TEST ID: BB-CALL-103
Priority: P0 (Critical)
Description: Self-call
Preconditions: Alice OFFHOOK
Command: call Alice
Expected Output: "denial"
Test Type: Negative

TEST ID: BB-CALL-104
Priority: P1 (High)
Description: Target in another call
Preconditions: Alice OFFHOOK, Bob-Carol talking
Command: call Bob
Expected Output: "busy"
Test Type: Negative

TEST ID: BB-CALL-105
Priority: P1 (High)
Description: Caller in call
Preconditions: Alice-Bob talking, Carol OFFHOOK
Command: call Carol
Expected Output: "busy"
Test Type: Negative

TEST ID: BB-CALL-106
Priority: P1 (High)
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
Priority: P0 (Critical)
Description: Add 3rd to 2-way call
Preconditions: Alice-Bob talking, Carol OFFHOOK
Command: conference Carol
Expected Output: "Carol joined the conference."
Test Type: Positive

TEST ID: BB-CONF-002
Priority: P1 (High)
Description: Conference by phone
Preconditions: Alice-Bob talking, Carol OFFHOOK
Command: conference 34567
Expected Output: "Carol joined the conference."
Test Type: Positive

--------------------------------------------------------------------------------

#### 3.4.2 Failed Conference

TEST ID: BB-CONF-101
Priority: P0 (Critical)
Description: No active call
Preconditions: No one in call
Command: conference Carol
Expected Output: "silence"
Test Type: Negative

TEST ID: BB-CONF-102
Priority: P0 (Critical)
Description: Target doesn't exist
Preconditions: Alice-Bob talking
Command: conference Unknown
Expected Output: "denial"
Test Type: Negative

TEST ID: BB-CONF-103
Priority: P1 (High)
Description: Self-conference
Preconditions: Alice in call
Command: conference Alice
Expected Output: "denial"
Test Type: Negative

TEST ID: BB-CONF-104
Priority: P1 (High)
Description: Target in another call
Preconditions: Alice-Bob talking, Carol-Dave talking
Command: conference Carol
Expected Output: "busy"
Test Type: Negative

TEST ID: BB-CONF-105
Priority: P1 (High)
Description: Target ONHOOK
Preconditions: Alice-Bob talking, Carol ONHOOK
Command: conference Carol
Expected Output: "silence"
Test Type: Negative

TEST ID: BB-CONF-106
Priority: P0 (Critical)
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
Priority: P0 (Critical)
Description: Transfer with acceptance
Preconditions: Alice-Bob talking, Carol ONHOOK
Command & Input: transfer Carol, transferer: Alice
Answer: y
Expected Output: Carol joined, Alice hears silence
Test Type: Positive

TEST ID: BB-XFER-002
Priority: P1 (High)
Description: Transfer declined
Preconditions: Alice-Bob talking, Carol ONHOOK
Command & Input: transfer Carol, transferer: Alice
Answer: n
Expected Output: Original call restored
Test Type: Positive

--------------------------------------------------------------------------------

#### 3.5.2 Failed Transfer

TEST ID: BB-XFER-101
Priority: P0 (Critical)
Description: Target doesn't exist
Preconditions: Alice-Bob talking
Command: transfer Unknown
Expected Output: "denial"
Test Type: Negative

TEST ID: BB-XFER-102
Priority: P0 (Critical)
Description: Transferer doesn't exist
Preconditions: Alice-Bob talking
Command & Input: transfer Carol, transferer: Unknown
Expected Output: "denial"
Test Type: Negative

TEST ID: BB-XFER-103
Priority: P1 (High)
Description: Transfer to self
Preconditions: Alice-Bob talking
Command & Input: transfer Alice, transferer: Alice
Expected Output: "denial"
Test Type: Negative

TEST ID: BB-XFER-104
Priority: P1 (High)
Description: Transferer not in call
Preconditions: Alice-Bob talking
Command & Input: transfer Carol, transferer: Dave
Expected Output: "denial"
Test Type: Negative

TEST ID: BB-XFER-105
Priority: P1 (High)
Description: From 3-way call
Preconditions: Alice-Bob-Carol talking
Command & Input: transfer Dave, transferer: Alice
Expected Output: "denial"
Test Type: Negative

TEST ID: BB-XFER-106
Priority: P1 (High)
Description: Target in another call
Preconditions: Alice-Bob talking, Carol-Dave talking
Command & Input: transfer Carol, transferer: Alice
Expected Output: "denial"
Test Type: Negative

TEST ID: BB-XFER-107
Priority: P1 (High)
Description: Target not ONHOOK
Preconditions: Alice-Bob talking, Carol OFFHOOK
Command & Input: transfer Carol, transferer: Alice
Expected Output: "denial"
Test Type: Negative

TEST ID: BB-XFER-108
Priority: P1 (High)
Description: Target in same call
Preconditions: Alice-Bob talking
Command & Input: transfer Bob, transferer: Alice
Expected Output: "already part of that call"
Test Type: Negative

================================================================================

### 3.6 SYSTEM COMMANDS (BLACK-BOX)

TEST ID: BB-SYS-001
Priority: P1 (High)
Description: Display help
Command: help
Expected Result: Help menu displayed
Test Type: Positive

TEST ID: BB-SYS-002
Priority: P2 (Low)
Description: Display help by number
Command: 7
Expected Result: Help menu displayed
Test Type: Positive

TEST ID: BB-SYS-003
Priority: P1 (High)
Description: Display status
Command: status
Expected Result: All phones and states shown
Test Type: Positive

TEST ID: BB-SYS-004
Priority: P2 (Low)
Description: Display status by number
Command: 6
Expected Result: All phones and states shown
Test Type: Positive

TEST ID: BB-SYS-005
Priority: P0 (Critical)
Description: Exit by quit
Command: quit
Expected Result: Command count displayed, program exits
Test Type: Positive

TEST ID: BB-SYS-006
Priority: P0 (Critical)
Description: Exit by exit
Command: exit
Expected Result: Command count displayed, program exits
Test Type: Positive

TEST ID: BB-SYS-007
Priority: P2 (Low)
Description: Exit by number
Command: 8
Expected Result: Command count displayed, program exits
Test Type: Positive

TEST ID: BB-SYS-008
Priority: P1 (High)
Description: Invalid command
Command: invalid
Expected Result: "Invalid command."
Test Type: Negative

================================================================================

### 3.7 INPUT HANDLING (BLACK-BOX)

TEST ID: BB-INPUT-001
Priority: P2 (Low)
Description: Leading whitespace
Input: "  offhook Alice"
Expected Behavior: Trimmed, processed normally
Test Type: Boundary

TEST ID: BB-INPUT-002
Priority: P2 (Low)
Description: Trailing whitespace
Input: "offhook Alice  "
Expected Behavior: Trimmed, processed normally
Test Type: Boundary

TEST ID: BB-INPUT-003
Priority: P2 (Low)
Description: Multiple spaces
Input: "offhook    Alice"
Expected Behavior: Processed normally
Test Type: Boundary

TEST ID: BB-INPUT-004
Priority: P1 (High)
Description: Exactly 50 characters
Input: 50-char command
Expected Behavior: Processed normally
Test Type: Boundary

TEST ID: BB-INPUT-005
Priority: P1 (High)
Description: 51 characters
Input: 51-char command
Expected Behavior: Truncated to 50, warning shown
Test Type: Boundary

TEST ID: BB-INPUT-006
Priority: P2 (Low)
Description: Empty input
Input: ""
Expected Behavior: Ignored, prompt again
Test Type: Boundary

TEST ID: BB-INPUT-007
Priority: P2 (Low)
Description: Whitespace only
Input: "   "
Expected Behavior: Ignored, prompt again
Test Type: Boundary

TEST ID: BB-INPUT-008
Priority: P1 (High)
Description: Case insensitive commands
Input: "OFFHOOK Alice"
Expected Behavior: Processed normally
Test Type: Positive

================================================================================

### 3.8 END-TO-END SCENARIOS (BLACK-BOX)

TEST ID: BB-E2E-001
Priority: P0 (Critical)
Description: Complete call lifecycle
Test Steps:
    1. Alice offhook
    2. Bob offhook
    3. call Bob
    4. onhook Alice
Expected Results: Each step succeeds with correct messages
Test Type: Integration

TEST ID: BB-E2E-002
Priority: P0 (Critical)
Description: Build 3-way conference
Test Steps:
    1. Alice & Bob offhook
    2. call Bob
    3. Carol offhook
    4. conference Carol
Expected Results: 3-way call established
Test Type: Integration

TEST ID: BB-E2E-003
Priority: P1 (High)
Description: Conference then dropout
Test Steps:
    1. Alice-Bob-Carol in 3-way
    2. onhook Bob
Expected Results: Alice-Carol continue in 2-way
Test Type: Integration

TEST ID: BB-E2E-004
Priority: P0 (Critical)
Description: Transfer workflow
Test Steps:
    1. Alice-Bob call
    2. transfer Carol (Alice transfers, Carol accepts)
Expected Results: Bob-Carol now talking
Test Type: Integration

TEST ID: BB-E2E-005
Priority: P1 (High)
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
Priority: P0 (Critical)
Description: File not found path
Target Method: LoadPhoneEntries
Execution Path: File.Exists returns false
Input: Invalid path
Expected Internal State: Returns null, usedPhoneNumbers empty
Test Type: Path Coverage

TEST ID: WB-FL-002
Priority: P1 (High)
Description: Empty usedPhoneNumbers set
Target Method: LoadPhoneEntries
Execution Path: No valid entries processed
Input: Empty file
Expected Internal State: usedPhoneNumbers.Count == 0
Test Type: Path Coverage

TEST ID: WB-FL-003
Priority: P1 (High)
Description: Comment line skip
Target Method: ProcessLine
Execution Path: IsEmptyOrComment returns true
Input: "# comment"
Expected Internal State: Returns null, sets not modified
Test Type: Branch Coverage

TEST ID: WB-FL-004
Priority: P2 (Low)
Description: Empty line skip
Target Method: ProcessLine
Execution Path: IsEmptyOrComment returns true
Input: ""
Expected Internal State: Returns null, sets not modified
Test Type: Branch Coverage

TEST ID: WB-FL-005
Priority: P1 (High)
Description: Split produces 1 part
Target Method: ProcessLine
Execution Path: parts.Length != 2 (less than)
Input: "12345"
Expected Internal State: Error message, returns null
Test Type: Branch Coverage

TEST ID: WB-FL-006
Priority: P1 (High)
Description: Split produces 3 parts
Target Method: ProcessLine
Execution Path: parts.Length != 2 (more than)
Input: "12345 Alice Bob"
Expected Internal State: Error message, returns null
Test Type: Branch Coverage

TEST ID: WB-FL-007
Priority: P1 (High)
Description: Phone validation failure
Target Method: ProcessLine
Execution Path: IsValidPhoneNumber returns false
Input: "1234 Alice"
Expected Internal State: Returns null, sets not modified
Test Type: Branch Coverage

TEST ID: WB-FL-008
Priority: P1 (High)
Description: Name validation failure
Target Method: ProcessLine
Execution Path: IsValidName returns false
Input: "12345 A"
Expected Internal State: Returns null, sets not modified
Test Type: Branch Coverage

TEST ID: WB-FL-009
Priority: P1 (High)
Description: Duplicate phone detected
Target Method: ProcessLine
Execution Path: IsDuplicatePhoneNumber returns true
Input: "12345 Bob" (after "12345 Alice")
Expected Internal State: Returns null, first entry kept
Test Type: Branch Coverage

TEST ID: WB-FL-010
Priority: P1 (High)
Description: Duplicate name detected
Target Method: ProcessLine
Execution Path: IsDuplicateName returns true
Input: "23456 Alice" (after "12345 Alice")
Expected Internal State: Returns null, first entry kept
Test Type: Branch Coverage

TEST ID: WB-FL-011
Priority: P0 (Critical)
Description: All validations pass
Target Method: ProcessLine
Execution Path: All validators return true/false correctly
Input: "12345 Alice"
Expected Internal State: Entry returned, both sets updated
Test Type: Path Coverage

TEST ID: WB-FL-012
Priority: P0 (Critical)
Description: Max entries boundary
Target Method: LoadPhoneEntries
Execution Path: validEntriesLoaded >= maxEntries
Input: 20 valid + 5 more
Expected Internal State: First 20 loaded, warning shown once
Test Type: Loop Coverage

--------------------------------------------------------------------------------

#### 4.1.2 Boundary Value Analysis

TEST ID: WB-FL-101
Priority: P1 (High)
Description: Phone length = 0
Method: IsValidPhoneNumber
Test Value: ""
Expected Result: false
Test Type: Boundary

TEST ID: WB-FL-102
Priority: P1 (High)
Description: Phone length = 1
Method: IsValidPhoneNumber
Test Value: "1"
Expected Result: false
Test Type: Boundary

TEST ID: WB-FL-103
Priority: P1 (High)
Description: Phone length = 4
Method: IsValidPhoneNumber
Test Value: "1234"
Expected Result: false
Test Type: Boundary

TEST ID: WB-FL-104
Priority: P0 (Critical)
Description: Phone length = 5
Method: IsValidPhoneNumber
Test Value: "12345"
Expected Result: true
Test Type: Boundary

TEST ID: WB-FL-105
Priority: P1 (High)
Description: Phone length = 6
Method: IsValidPhoneNumber
Test Value: "123456"
Expected Result: false
Test Type: Boundary

TEST ID: WB-FL-106
Priority: P1 (High)
Description: Name length = 0
Method: IsValidName
Test Value: ""
Expected Result: false
Test Type: Boundary

TEST ID: WB-FL-107
Priority: P0 (Critical)
Description: Name length = 1
Method: IsValidName
Test Value: "A"
Expected Result: true
Test Type: Boundary

TEST ID: WB-FL-108
Priority: P0 (Critical)
Description: Name length = 12
Method: IsValidName
Test Value: "AliceAndBobs"
Expected Result: true
Test Type: Boundary

TEST ID: WB-FL-109
Priority: P1 (High)
Description: Name length = 13
Method: IsValidName
Test Value: "AliceAndBobsy"
Expected Result: false
Test Type: Boundary

TEST ID: WB-FL-110
Priority: P0 (Critical)
Description: Max entries = 20
Method: LoadPhoneEntries
Test Value: 20 entries
Expected Result: All loaded, no warning
Test Type: Boundary

TEST ID: WB-FL-111
Priority: P0 (Critical)
Description: Max entries = 21
Method: LoadPhoneEntries
Test Value: 21 entries
Expected Result: 20 loaded, warning shown
Test Type: Boundary

--------------------------------------------------------------------------------

#### 4.1.3 Data Structure Validation

TEST ID: WB-FL-201
Priority: P1 (High)
Description: usedPhoneNumbers populated
Method: ProcessLine
Test Scenario: Process valid entry
Verification: Contains("12345") == true
Test Type: State Verification

TEST ID: WB-FL-202
Priority: P1 (High)
Description: usedNames case-insensitive
Method: ProcessLine
Test Scenario: Add "Alice" and "ALICE"
Verification: usedNames.Contains("alice") for both
Test Type: State Verification

TEST ID: WB-FL-203
Priority: P1 (High)
Description: HashSet prevents duplicates
Method: ProcessLine
Test Scenario: Add same phone twice
Verification: usedPhoneNumbers.Count == 1
Test Type: State Verification

TEST ID: WB-FL-204
Priority: P2 (Low)
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
Priority: P0 (Critical)
Description: Initialize phoneStates
Method: Constructor
Execution Path: foreach loop over phoneBook
Input: 5 entries
Expected Internal State: phoneStates.Count == 5, all ONHOOK
Test Type: Path Coverage

TEST ID: WB-PS-002
Priority: P1 (High)
Description: SetPhoneState - exists
Method: SetPhoneState
Execution Path: ContainsKey returns true
Input: Valid phone
Expected Internal State: phoneStates[phone] updated
Test Type: Branch Coverage

TEST ID: WB-PS-003
Priority: P1 (High)
Description: SetPhoneState - not exists
Method: SetPhoneState
Execution Path: ContainsKey returns false
Input: Invalid phone
Expected Internal State: phoneStates unchanged
Test Type: Branch Coverage

TEST ID: WB-PS-004
Priority: P1 (High)
Description: GetPhoneState - exists
Method: GetPhoneState
Execution Path: TryGetValue returns true
Input: Valid phone
Expected Internal State: Returns stored state
Test Type: Branch Coverage

TEST ID: WB-PS-005
Priority: P1 (High)
Description: GetPhoneState - not exists
Method: GetPhoneState
Execution Path: TryGetValue returns false
Input: Invalid phone
Expected Internal State: Returns ONHOOK default
Test Type: Branch Coverage

TEST ID: WB-PS-006
Priority: P1 (High)
Description: FindEntry - by phone first
Method: FindEntry
Execution Path: First foreach matches
Input: "12345"
Expected Internal State: Returns entry, second loop not executed
Test Type: Path Coverage

TEST ID: WB-PS-007
Priority: P1 (High)
Description: FindEntry - by name second
Method: FindEntry
Execution Path: Second foreach matches
Input: "Alice"
Expected Internal State: First loop completes, then returns from second
Test Type: Path Coverage

TEST ID: WB-PS-008
Priority: P1 (High)
Description: FindEntry - not found
Method: FindEntry
Execution Path: Both loops complete
Input: "Unknown"
Expected Internal State: Returns null
Test Type: Path Coverage

TEST ID: WB-PS-009
Priority: P1 (High)
Description: FindEntry - case insensitive
Method: FindEntry
Execution Path: Name comparison with StringComparison.OrdinalIgnoreCase
Input: "alice"
Expected Internal State: Matches "Alice"
Test Type: Branch Coverage

--------------------------------------------------------------------------------

#### 4.2.2 Call Management - activeCalls List

TEST ID: WB-PS-101
Priority: P0 (Critical)
Description: StartCall creates HashSet
Method: StartCall
Execution Path: New HashSet added to list
Precondition: Empty activeCalls
Expected activeCalls State: activeCalls.Count == 1, call.Count == 2
Test Type: State Verification

TEST ID: WB-PS-102
Priority: P0 (Critical)
Description: StartCall updates both states
Method: StartCall
Execution Path: SetPhoneState called twice
Precondition: Valid phones
Expected activeCalls State: Both phones TALKING_2WAY
Test Type: State Verification

TEST ID: WB-PS-103
Priority: P1 (High)
Description: IsPhoneInCall - true path
Method: IsPhoneInCall
Execution Path: Any returns true
Precondition: Phone in call
Expected activeCalls State: Returns true
Test Type: Branch Coverage

TEST ID: WB-PS-104
Priority: P1 (High)
Description: IsPhoneInCall - false path
Method: IsPhoneInCall
Execution Path: Any returns false
Precondition: Phone not in call
Expected activeCalls State: Returns false
Test Type: Branch Coverage

TEST ID: WB-PS-105
Priority: P1 (High)
Description: GetCallForPhone - found
Method: GetCallForPhone
Execution Path: FirstOrDefault returns call
Precondition: Phone in call
Expected activeCalls State: Returns HashSet reference
Test Type: Branch Coverage

TEST ID: WB-PS-106
Priority: P1 (High)
Description: GetCallForPhone - not found
Method: GetCallForPhone
Execution Path: FirstOrDefault returns null
Precondition: Phone not in call
Expected activeCalls State: Returns null
Test Type: Branch Coverage

TEST ID: WB-PS-107
Priority: P0 (Critical)
Description: TryAddToCall - success
Method: TryAddToCall
Execution Path: call != null && call.Count < 3
Precondition: 2-way call exists
Expected activeCalls State: call.Count == 3, all TALKING_3WAY
Test Type: Path Coverage

TEST ID: WB-PS-108
Priority: P1 (High)
Description: TryAddToCall - null call
Method: TryAddToCall
Execution Path: call == null
Precondition: Phone not in call
Expected activeCalls State: Returns false, no changes
Test Type: Branch Coverage

TEST ID: WB-PS-109
Priority: P0 (Critical)
Description: TryAddToCall - call full
Method: TryAddToCall
Execution Path: call.Count >= 3
Precondition: 3-way call exists
Expected activeCalls State: Returns false, no changes
Test Type: Branch Coverage

--------------------------------------------------------------------------------

#### 4.2.3 LeaveCall Logic Paths

TEST ID: WB-PS-201
Priority: P1 (High)
Description: LeaveCall - null call
Method: LeaveCall
Execution Path: call == null, early return
Precondition: Phone not in call
Expected State After: No changes, no console output
Test Type: Path Coverage

TEST ID: WB-PS-202
Priority: P0 (Critical)
Description: LeaveCall - 1 remaining
Method: LeaveCall
Execution Path: call.Count == 1 after remove
Precondition: 2-way call
Expected State After: activeCalls.Count decreases, remaining OFFHOOK
Test Type: Path Coverage

TEST ID: WB-PS-203
Priority: P0 (Critical)
Description: LeaveCall - 2 remaining
Method: LeaveCall
Execution Path: call.Count == 2 after remove
Precondition: 3-way call
Expected State After: Both remaining TALKING_2WAY
Test Type: Path Coverage

TEST ID: WB-PS-204
Priority: P2 (Low)
Description: LeaveCall - 0 remaining
Method: LeaveCall
Execution Path: call.Count == 0 after remove
Precondition: Edge case
Expected State After: activeCalls removes empty set
Test Type: Path Coverage

TEST ID: WB-PS-205
Priority: P1 (High)
Description: LeaveCall removes from set
Method: LeaveCall
Execution Path: call.Remove called
Precondition: Phone in call
Expected State After: call.Contains(phone) == false
Test Type: State Verification

TEST ID: WB-PS-206
Priority: P1 (High)
Description: LeaveCall updates state
Method: LeaveCall
Execution Path: SetPhoneState to ONHOOK
Precondition: Any call
Expected State After: phoneStates[phone] == ONHOOK
Test Type: State Verification

--------------------------------------------------------------------------------

#### 4.2.4 TryTransferCall Logic Paths

TEST ID: WB-PS-301
Priority: P1 (High)
Description: Transfer - null call
Method: TryTransferCall
Execution Path: call == null
Precondition: Transferer not in call
Expected State: Returns false, no changes
Test Type: Branch Coverage

TEST ID: WB-PS-302
Priority: P0 (Critical)
Description: Transfer - not 2-way
Method: TryTransferCall
Execution Path: call.Count != 2
Precondition: 3-way call
Expected State: Returns false, no changes
Test Type: Branch Coverage

TEST ID: WB-PS-303
Priority: P1 (High)
Description: Transfer - target in call
Method: TryTransferCall
Execution Path: IsPhoneInCall(target) == true
Precondition: Target busy
Expected State: Returns false, no changes
Test Type: Branch Coverage

TEST ID: WB-PS-304
Priority: P1 (High)
Description: Transfer - target not in phoneStates
Method: TryTransferCall
Execution Path: TryGetValue returns false
Precondition: Invalid target
Expected State: Returns false, no changes
Test Type: Branch Coverage

TEST ID: WB-PS-305
Priority: P1 (High)
Description: Transfer - target not ONHOOK
Method: TryTransferCall
Execution Path: targetState != ONHOOK
Precondition: Target OFFHOOK
Expected State: Returns false, no changes
Test Type: Branch Coverage

TEST ID: WB-PS-306
Priority: P0 (Critical)
Description: Transfer accepted - y path
Method: TryTransferCall
Execution Path: response == "y"
Precondition: Valid transfer
Expected State: Target in call, transferer out, returns true
Test Type: Path Coverage

TEST ID: WB-PS-307
Priority: P1 (High)
Description: Transfer declined - else path
Method: TryTransferCall
Execution Path: response != "y"
Precondition: Valid transfer
Expected State: Original call restored, returns false
Test Type: Path Coverage

TEST ID: WB-PS-308
Priority: P2 (Low)
Description: Transfer updates other party
Method: TryTransferCall
Execution Path: call.First(p => p != transferer)
Precondition: Valid transfer
Expected State: Other party correctly identified
Test Type: Logic Verification

TEST ID: WB-PS-309
Priority: P1 (High)
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
Priority: P0 (Critical)
Description: Offhook - entry null
Method: HandleOffhook
Execution Path: entry == null
Precondition: Invalid identifier
Expected Flow: Prints "denial", returns early
Test Type: Path Coverage

TEST ID: WB-CP-002
Priority: P1 (High)
Description: Offhook - not ONHOOK
Method: HandleOffhook
Execution Path: state != ONHOOK
Precondition: Phone already offhook
Expected Flow: Prints "already offhook", returns
Test Type: Path Coverage

TEST ID: WB-CP-003
Priority: P0 (Critical)
Description: Offhook - success
Method: HandleOffhook
Execution Path: Both checks pass
Precondition: Valid ONHOOK phone
Expected Flow: Sets OFFHOOK_DIALTONE, prints success
Test Type: Path Coverage

--------------------------------------------------------------------------------

#### 4.3.2 HandleOnhook Paths

TEST ID: WB-CP-101
Priority: P0 (Critical)
Description: Onhook - entry null
Method: HandleOnhook
Execution Path: entry == null
Precondition: Invalid identifier
Expected Flow: Prints "denial", returns early
Test Type: Path Coverage

TEST ID: WB-CP-102
Priority: P0 (Critical)
Description: Onhook - in call
Method: HandleOnhook
Execution Path: IsPhoneInCall == true
Precondition: Phone in active call
Expected Flow: Calls LeaveCall, prints "hung up"
Test Type: Branch Coverage

TEST ID: WB-CP-103
Priority: P1 (High)
Description: Onhook - already onhook
Method: HandleOnhook
Execution Path: state == ONHOOK
Precondition: Phone already onhook
Expected Flow: Prints "already onhook"
Test Type: Branch Coverage

TEST ID: WB-CP-104
Priority: P0 (Critical)
Description: Onhook - success
Method: HandleOnhook
Execution Path: All checks pass
Precondition: Phone offhook, not in call
Expected Flow: Sets ONHOOK, prints success
Test Type: Path Coverage

--------------------------------------------------------------------------------

#### 4.3.3 HandleCall Complex Logic

TEST ID: WB-CP-201
Priority: P0 (Critical)
Description: Call - target null
Method: HandleCall
Execution Path: FindEntry returns null
Precondition: Invalid target
Expected Flow: Prints "denial", returns
Test Type: Path Coverage

TEST ID: WB-CP-202
Priority: P0 (Critical)
Description: Call - caller null
Method: HandleCall
Execution Path: FindEntry(callerInput) returns null
Precondition: Invalid caller
Expected Flow: Prints "denial", returns
Test Type: Path Coverage

TEST ID: WB-CP-203
Priority: P1 (High)
Description: Call - self call
Method: HandleCall
Execution Path: caller.Phone == target.Phone
Precondition: Same phone
Expected Flow: Prints "denial", returns
Test Type: Branch Coverage

TEST ID: WB-CP-204
Priority: P1 (High)
Description: Call - target in call
Method: HandleCall
Execution Path: IsPhoneInCall(target) == true
Precondition: Target busy
Expected Flow: Prints "busy", returns
Test Type: Branch Coverage

TEST ID: WB-CP-205
Priority: P1 (High)
Description: Call - caller in call
Method: HandleCall
Execution Path: IsPhoneInCall(caller) == true
Precondition: Caller busy
Expected Flow: Prints "busy", returns
Test Type: Branch Coverage

TEST ID: WB-CP-206
Priority: P1 (High)
Description: Call - target ONHOOK
Method: HandleCall
Execution Path: GetPhoneState == ONHOOK
Precondition: Target onhook
Expected Flow: Prints "silence", returns
Test Type: Branch Coverage

TEST ID: WB-CP-207
Priority: P0 (Critical)
Description: Call - success
Method: HandleCall
Execution Path: All checks pass
Precondition: Valid call scenario
Expected Flow: Calls StartCall, prints success
Test Type: Path Coverage

--------------------------------------------------------------------------------

#### 4.3.4 HandleTransfer Complex Logic

TEST ID: WB-CP-301
Priority: P0 (Critical)
Description: Transfer - target null
Method: HandleTransfer
Execution Path: FindEntry(identifier) == null
Precondition: Invalid target
Expected Flow: Prints "denial", returns
Test Type: Path Coverage

TEST ID: WB-CP-302
Priority: P1 (High)
Description: Transfer - empty input
Method: HandleTransfer
Execution Path: IsNullOrWhiteSpace(input)
Precondition: User enters nothing
Expected Flow: Prints "denial", returns
Test Type: Branch Coverage

TEST ID: WB-CP-303
Priority: P0 (Critical)
Description: Transfer - transferer null
Method: HandleTransfer
Execution Path: FindEntry(transferer) == null
Precondition: Invalid transferer
Expected Flow: Prints "denial", returns
Test Type: Path Coverage

TEST ID: WB-CP-304
Priority: P1 (High)
Description: Transfer - self transfer
Method: HandleTransfer
Execution Path: transferer.Phone == target.Phone
Precondition: Same phone
Expected Flow: Prints "denial", returns
Test Type: Branch Coverage

TEST ID: WB-CP-305
Priority: P0 (Critical)
Description: Transfer - not 2-way call
Method: HandleTransfer
Execution Path: call == null || call.Count != 2
Precondition: Not in 2-way
Expected Flow: Prints "denial", returns
Test Type: Branch Coverage

TEST ID: WB-CP-306
Priority: P1 (High)
Description: Transfer - target in same call
Method: HandleTransfer
Execution Path: call.Contains(target.Phone)
Precondition: Target already present
Expected Flow: Prints "already part", returns
Test Type: Branch Coverage

TEST ID: WB-CP-307
Priority: P1 (High)
Description: Transfer - TryTransfer false
Method: HandleTransfer
Execution Path: TryTransferCall returns false
Precondition: Transfer validation fails
Expected Flow: Prints "denial", returns
Test Type: Path Coverage

TEST ID: WB-CP-308
Priority: P0 (Critical)
Description: Transfer - success
Method: HandleTransfer
Execution Path: All checks pass
Precondition: Valid scenario
Expected Flow: Calls TryTransferCall, completes
Test Type: Path Coverage

--------------------------------------------------------------------------------

#### 4.3.5 HandleConference Logic

TEST ID: WB-CP-401
Priority: P1 (High)
Description: Conference - no calls
Method: HandleConference
Execution Path: GetTwoWayCalls().Count == 0
Precondition: No one in call
Expected Flow: Prints "silence", returns
Test Type: Path Coverage

TEST ID: WB-CP-402
Priority: P0 (Critical)
Description: Conference - participant null
Method: HandleConference
Execution Path: FindEntry == null
Precondition: Invalid participant
Expected Flow: Prints "denial", returns
Test Type: Path Coverage

TEST ID: WB-CP-403
Priority: P1 (High)
Description: Conference - participant in call
Method: HandleConference
Execution Path: IsPhoneInCall(participant)
Precondition: Participant busy
Expected Flow: Prints "busy", returns
Test Type: Branch Coverage

TEST ID: WB-CP-404
Priority: P1 (High)
Description: Conference - participant ONHOOK
Method: HandleConference
Execution Path: GetPhoneState == ONHOOK
Precondition: Participant onhook
Expected Flow: Prints "silence", returns
Test Type: Branch Coverage

TEST ID: WB-CP-405
Priority: P0 (Critical)
Description: Conference - TryAdd success
Method: HandleConference
Execution Path: TryAddToCall returns true
Precondition: Valid add
Expected Flow: Prints "joined", success
Test Type: Path Coverage

TEST ID: WB-CP-406
Priority: P0 (Critical)
Description: Conference - TryAdd fails
Method: HandleConference
Execution Path: TryAddToCall returns false
Precondition: Conference full
Expected Flow: Prints "busy (full)", returns
Test Type: Path Coverage

TEST ID: WB-CP-407
Priority: P1 (High)
Description: Conference - multiple calls menu
Method: HandleConference
Execution Path: twoWayCalls.Count > 1
Precondition: Multiple 2-way calls active
Expected Flow: Shows menu, user selects
Test Type: Path Coverage

================================================================================

### 4.4 USERINTERFACE CLASS (WHITE-BOX)

--------------------------------------------------------------------------------

#### 4.4.1 GetUserInput Logic

TEST ID: WB-UI-001
Priority: P1 (High)
Description: Null input
Method: GetUserInput
Execution Path: ReadLine returns null
Input: Ctrl+D/EOF
Expected Internal Processing: Returns null
Test Type: Branch Coverage

TEST ID: WB-UI-002
Priority: P2 (Low)
Description: Input requires trim
Method: GetUserInput
Execution Path: Trim called
Input: "  text  "
Expected Internal Processing: Leading/trailing removed
Test Type: Logic Verification

TEST ID: WB-UI-003
Priority: P1 (High)
Description: Length check > 50
Method: GetUserInput
Execution Path: Length > 50 branch
Input: 60 chars
Expected Internal Processing: Substring(0,50), warning printed
Test Type: Branch Coverage

TEST ID: WB-UI-004
Priority: P1 (High)
Description: Length check <= 50
Method: GetUserInput
Execution Path: Length <= 50 branch
Input: 50 chars
Expected Internal Processing: No truncation, no warning
Test Type: Branch Coverage

TEST ID: WB-UI-005
Priority: P2 (Low)
Description: Empty after trim
Method: GetUserInput
Execution Path: IsNullOrWhiteSpace after trim
Input: "   "
Expected Internal Processing: Returns null
Test Type: Branch Coverage

TEST ID: WB-UI-006
Priority: P1 (High)
Description: Valid after trim
Method: GetUserInput
Execution Path: Returns trimmed string
Input: "offhook Alice"
Expected Internal Processing: Returns trimmed value
Test Type: Path Coverage

--------------------------------------------------------------------------------

#### 4.4.2 PrintStatus Reflection Logic

TEST ID: WB-UI-101
Priority: P1 (High)
Description: Reflection success
Method: PrintStatus
Execution Path: GetField returns field
Scenario: Normal call
Expected Internal Behavior: activeCalls retrieved via reflection
Test Type: Path Coverage

TEST ID: WB-UI-102
Priority: P2 (Low)
Description: Reflection failure
Method: PrintStatus
Execution Path: GetField returns null
Scenario: Field renamed
Expected Internal Behavior: Uses empty list, no crash
Test Type: Branch Coverage

TEST ID: WB-UI-103
Priority: P1 (High)
Description: Call count = 0
Method: PrintStatus
Execution Path: call == null path
Scenario: Phone not in call
Expected Internal Behavior: Prints simple state
Test Type: Branch Coverage

TEST ID: WB-UI-104
Priority: P0 (Critical)
Description: Call count = 2
Method: PrintStatus
Execution Path: call.Count == 2 path
Scenario: 2-way call
Expected Internal Behavior: Finds other, prints TALKING_2WAY
Test Type: Branch Coverage

TEST ID: WB-UI-105
Priority: P0 (Critical)
Description: Call count = 3
Method: PrintStatus
Execution Path: call.Count == 3 path
Scenario: 3-way call
Expected Internal Behavior: Uses Where and Select, prints TALKING_3WAY
Test Type: Branch Coverage

TEST ID: WB-UI-106
Priority: P2 (Low)
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
Priority: P1 (High)
Description: GetUserInput null
Section: Main loop
Execution Path: input == null
Input: EOF
Expected Routing: Breaks loop
Test Type: Branch Coverage

TEST ID: WB-MAIN-002
Priority: P2 (Low)
Description: Command count increment
Section: Main loop
Execution Path: commandCount++
Input: Any command
Expected Routing: Counter increases
Test Type: Logic Verification

TEST ID: WB-MAIN-003
Priority: P1 (High)
Description: Case conversion
Section: Switch prep
Execution Path: cmd = parts[0].ToLower()
Input: "OFFHOOK"
Expected Routing: Converted to "offhook"
Test Type: Logic Verification

TEST ID: WB-MAIN-004
Priority: P1 (High)
Description: Help by name
Section: Switch case
Execution Path: case "help"
Input: "help"
Expected Routing: Calls PrintHelp
Test Type: Path Coverage

TEST ID: WB-MAIN-005
Priority: P2 (Low)
Description: Help by number
Section: Switch case
Execution Path: case "7"
Input: "7"
Expected Routing: Calls PrintHelp
Test Type: Path Coverage

TEST ID: WB-MAIN-006
Priority: P0 (Critical)
Description: Quit returns
Section: Switch case
Execution Path: case "quit" return
Input: "quit"
Expected Routing: ShowExitMessage, exits
Test Type: Path Coverage

TEST ID: WB-MAIN-007
Priority: P1 (High)
Description: Parts validation
Section: Switch case
Execution Path: parts.Length != 2
Input: "offhook"
Expected Routing: Prints "Invalid syntax"
Test Type: Branch Coverage

TEST ID: WB-MAIN-008
Priority: P1 (High)
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
Priority: P0 (Critical)
Description: phoneStates and activeCalls sync
Operations: StartCall
Verification Point: After call start
Expected Consistency: Both in phoneStates and activeCalls
Test Type: State Consistency

TEST ID: WB-INT-002
Priority: P0 (Critical)
Description: LeaveCall cleanup
Operations: Leave 2-way call
Verification Point: After leave
Expected Consistency: Removed from activeCalls, state updated
Test Type: State Consistency

TEST ID: WB-INT-003
Priority: P1 (High)
Description: State transition validity
Operations: ONHOOK -> OFFHOOK -> TALKING
Verification Point: Each step
Expected Consistency: No invalid state transitions
Test Type: State Machine

TEST ID: WB-INT-004
Priority: P1 (High)
Description: HashSet reference consistency
Operations: TryAddToCall
Verification Point: After add
Expected Consistency: Same HashSet in activeCalls and GetCallForPhone
Test Type: Reference Integrity

TEST ID: WB-INT-005
Priority: P1 (High)
Description: activeCalls count
Operations: Multiple operations
Verification Point: After each op
Expected Consistency: Count matches actual calls
Test Type: State Verification

--------------------------------------------------------------------------------

#### 4.6.2 Data Structure Integrity

TEST ID: WB-INT-101
Priority: P1 (High)
Description: HashSet uniqueness
Scenario: Add duplicate to call
Verification: Check Contains
Expected Result: No duplicates in HashSet
Test Type: Structure Validation

TEST ID: WB-INT-102
Priority: P1 (High)
Description: Dictionary key existence
Scenario: SetPhoneState with unknown
Verification: Check ContainsKey
Expected Result: No exception, silent fail
Test Type: Exception Prevention

TEST ID: WB-INT-103
Priority: P2 (Low)
Description: List ordering
Scenario: Add multiple calls
Verification: Iterate activeCalls
Expected Result: Maintains insertion order
Test Type: Structure Validation

TEST ID: WB-INT-104
Priority: P1 (High)
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

### 5.1 SMOKE TESTING

**Purpose**: Quick verification that critical functionality works before full testing begins

**Scope**: P0 test cases only (Critical priority)

**Test Cases**: ~40 smoke tests covering:
- File loading (BB-FL-001, BB-FL-101)
- Basic offhook/onhook (BB-OH-001, BB-ON-001)
- Call establishment (BB-CALL-001, BB-CALL-102)
- Conference calls (BB-CONF-001, BB-CONF-106)
- Call transfer (BB-XFER-001)
- System exit (BB-SYS-005)

**Duration**: 30 minutes  
**Frequency**: Beginning of each test phase  
**Exit Criteria**: All P0 tests pass

--------------------------------------------------------------------------------

### 5.2 MANUAL TESTING (BLACK-BOX)

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
- Defect tracking system

TEST EXECUTION ORDER:
1. Smoke tests (P0 only)
2. File loading tests (all priorities)
3. Command operations (P0, then P1, then P2)
4. End-to-end scenarios
5. Regression testing after defect fixes

--------------------------------------------------------------------------------

### 5.3 AUTOMATED UNIT TESTING (WHITE-BOX)

PURPOSE: Validate internal logic and code paths

APPROACH:

Example test structure:

```csharp
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
```

FRAMEWORKS:
- MSTest 

TEST EXECUTION ORDER:
1. PhoneFileLoader tests (foundation)
2. PhoneSystem tests (core logic)
3. CommandProcessor tests (business logic)
4. UserInterface tests (presentation)
5. Integration tests (cross-component)

--------------------------------------------------------------------------------

### 5.4 INTEGRATION TESTING (MIXED)

PURPOSE: Validate component interactions

APPROACH:
- Test PhoneSystem + CommandProcessor integration
- Test file loading to system initialization
- Verify state consistency across components

TEST CASES:
- End-to-end scenarios (BB-E2E series)
- State synchronization tests (WB-INT series)
- Multi-component workflows

--------------------------------------------------------------------------------

### 5.5 REGRESSION TESTING

PURPOSE: Ensure fixes don't break existing functionality

APPROACH:
- Re-run all P0 tests after any code change
- Re-run affected P1 tests for related changes
- Full regression suite before release

FREQUENCY:
- After each defect fix
- Daily during active development
- Before each milestone

AUTOMATION:
- Automated unit tests run on every build
- Manual regression checklist for critical paths

--------------------------------------------------------------------------------

### 5.6 CODE COVERAGE ANALYSIS

TOOLS: Visual Studio Code Coverage or Coverlet

PROCESS:
1. Run all automated tests with coverage enabled
2. Generate coverage report
3. Identify uncovered branches
4. Add tests for uncovered paths
5. Target 90%+ coverage for critical classes

METRICS TO TRACK:
- Line coverage percentage
- Branch coverage percentage
- Method coverage percentage
- Class coverage percentage

================================================================================

## 6. TEST DATA REQUIREMENTS

### 6.1 BLACK-BOX TEST DATA

FILE: valid_phones.txt
CONTENTS: 5 standard entries
USED FOR: BB-FL-001, positive test scenarios

FILE: max_phones.txt
CONTENTS: Exactly 20 entries
USED FOR: BB-FL-004, boundary testing

FILE: over_max_phones.txt
CONTENTS: 25 entries
USED FOR: BB-FL-111, max entries validation

FILE: invalid_formats.txt
CONTENTS: Various format violations
USED FOR: BB-FL-103 through BB-FL-108

FILE: duplicates.txt
CONTENTS: Duplicate names and numbers
USED FOR: BB-FL-109, BB-FL-110

FILE: comments.txt
CONTENTS: File with # comments and empty lines
USED FOR: BB-FL-002, BB-FL-003

--------------------------------------------------------------------------------

### 6.2 WHITE-BOX TEST DATA

- Boundary value files (4,5,6 digit phones)
- Name length boundaries (1,12,13 characters)
- Files triggering specific code paths
- Malformed CSV structures
- Edge case scenarios

--------------------------------------------------------------------------------

### 6.3 TEST DATA STORAGE

LOCATION: /TestData directory in project root

ORGANIZATION:
```
/TestData
  /Valid
    - standard_5_entries.txt
    - max_20_entries.txt
  /Invalid
    - invalid_phone_length.txt
    - invalid_name_format.txt
    - duplicates.txt
  /Boundary
    - exactly_20.txt
    - over_max_25.txt
  /Edge
    - empty_file.txt
    - comments_only.txt
```

================================================================================

## 7. DEFECT CLASSIFICATION

### 7.1 BLACK-BOX DEFECTS

**SEVERITY LEVELS**:
- **S1 (Critical)**: Application crashes, data corruption, complete feature failure
- **S2 (High)**: Major functionality broken, incorrect results, no workaround
- **S3 (Medium)**: Minor functionality issues, workaround available
- **S4 (Low)**: Cosmetic issues, typos, minor UI problems

**EXAMPLES**:
- S1: Application crashes on file load
- S2: Wrong error messages displayed
- S3: Case sensitivity not working as documented
- S4: Help text has typos

--------------------------------------------------------------------------------

### 7.2 WHITE-BOX DEFECTS

**CATEGORIES**:
- Logic errors in conditions
- State inconsistencies
- Memory leaks (uncleaned activeCalls)
- Unreachable code
- Missing null checks
- Incorrect exception handling

**SEVERITY MAPPING**:
- Data structure corruption: S1
- State inconsistency: S1-S2
- Missing null check: S2-S3
- Code coverage gap: S3-S4

================================================================================

## 8. RESOURCE REQUIREMENTS

### 8.2 INFRASTRUCTURE RESOURCES

**DEVELOPMENT MACHINES**: 4 workstations
- CPU: Intel i5 or equivalent
- RAM: 8GB minimum
- OS: Windows 10/11 or Linux
- Disk: 50GB free space

**SOFTWARE TOOLS**:
- .NET 9.0 SDK
- Visual Studio 2022 or VS Code
- MSTest or NUnit framework
- Coverlet or dotCover (code coverage)
- Git (version control)
- Defect tracking system (Jira, Azure DevOps, or similar)

**TEST ENVIRONMENTS**:
- Local development environments (4)
- No special server infrastructure needed

--------------------------------------------------------------------------------

### 8.3 TEST DATA RESOURCES

As defined in Section 6:
- Valid test data files (6 files)
- Invalid test data files (8 files)
- Boundary test data files (4 files)
- Edge case test data files (3 files)

**STORAGE**: ~1MB total for all test data files

================================================================================

## 9. TEST SCHEDULE AND TIMELINE

### 9.1 OVERALL TIMELINE

**Duration**: 4 weeks (20 working days)  
**Start Date**: [To Be Determined]  
**End Date**: [To Be Determined]  
**Team**: 7 people (1 lead, 2 testers, 2 engineers, 2 developers)

### 9.2 DETAILED PHASE BREAKDOWN

--------------------------------------------------------------------------------

#### PHASE 1: TEST PREPARATION (Days 1-5)

**Duration**: 5 working days (1 week)

**Activities**:
- Set up test environments (4 workstations)
- Install and configure tools (.NET, Visual Studio, test frameworks)
- Create all test data files (21 files)
- Review and finalize test plan
- Set up defect tracking system
- Train team on test procedures

**Deliverables**:
- Test environment setup complete
- All test data files created and validated
- Test execution checklist prepared
- Team trained and ready

**Entry Criteria**:
- Test plan approved by stakeholders
- Development environment available
- Test team assigned

**Exit Criteria**:
- All 4 workstations configured and operational
- All test tools installed and verified
- Test data files created and accessible
- Team training completed
- Smoke test can be executed successfully

**Milestones**:
- Day 3: Environment setup complete
- Day 5: Test preparation complete, ready to begin testing

**Resources**:
- Test Lead: 10 hours
- Manual Testers: 8 hours each
- Automation Engineers: 12 hours each
- Total: 50 person-hours

--------------------------------------------------------------------------------

#### PHASE 2: UNIT TESTING (Days 6-10)

**Duration**: 5 working days (1 week)

**Activities**:
- Execute all white-box unit tests (WB series)
- Run code coverage analysis
- Review coverage reports
- Add tests for uncovered code paths
- Document and log defects
- Developers fix critical (P0) defects
- Retest fixed defects

**Test Cases Executed**:
- PhoneFileLoader: 30+ tests
- PhoneSystem: 35+ tests
- CommandProcessor: 30+ tests
- UserInterface: 15+ tests
- Program Main: 10+ tests
- **Total**: ~120 white-box tests

**Deliverables**:
- Unit test execution report
- Code coverage report (target: 90%+)
- Defect log for unit test phase
- Updated test cases for gaps

**Entry Criteria**:
- Test preparation phase complete
- Application code available and buildable
- Unit test framework configured
- Build passing without compilation errors

**Exit Criteria**:
- 90% code coverage achieved for critical classes
- All P0 unit test defects resolved
- All unit tests passing (100% pass rate)
- Code coverage report reviewed and accepted

**Milestones**:
- Day 7: First coverage report generated
- Day 9: 90% coverage achieved
- Day 10: All P0 defects fixed and unit tests passing

**Resources**:
- Automation Engineers: 30 hours each
- Developers: 15 hours each
- Test Lead: 8 hours
- Total: 98 person-hours

**Risks**: See Section 10.3 (R-01, R-05)

--------------------------------------------------------------------------------

#### PHASE 3: FUNCTIONAL TESTING (Days 11-15)

**Duration**: 5 working days (1 week)

**Activities**:
- Execute smoke tests (P0 black-box tests)
- Execute all black-box functional tests
- Priority order: P0 → P1 → P2
- Log defects in tracking system
- Daily defect triage meetings
- Developers fix P0 and P1 defects
- Retest all fixed defects
- Update test status dashboard

**Test Cases Executed**:
- File Loading: 15 tests (BB-FL series)
- Basic Commands: 20 tests (BB-OH, BB-ON series)
- Call Establishment: 10 tests (BB-CALL series)
- Conference Calls: 10 tests (BB-CONF series)
- Call Transfer: 10 tests (BB-XFER series)
- System Commands: 8 tests (BB-SYS series)
- Input Handling: 8 tests (BB-INPUT series)
- **Total**: ~80 black-box tests

**Deliverables**:
- Functional test execution report
- Defect summary report
- Test metrics dashboard
- Updated test cases (if needed)

**Entry Criteria**:
- Unit testing phase complete (90% coverage)
- All P0 unit test defects resolved
- Application build stable
- Test data files ready

**Exit Criteria**:
- All P0 functional tests passing
- All P1 defects resolved or scheduled for next release
- Test execution rate: 95%+ of planned tests completed
- Defect resolution rate: 100% of P0, 90%+ of P1

**Milestones**:
- Day 11: Smoke tests complete (P0 only)
- Day 13: All P0 tests executed
- Day 14: All P0 & P1 tests executed
- Day 15: All defects triaged, P0/P1 fixed

**Resources**:
- Manual Testers: 30 hours each
- Developers: 20 hours each
- Test Lead: 10 hours
- Total: 110 person-hours

**Risks**: See Section 10.3 (R-03, R-06)

--------------------------------------------------------------------------------

#### PHASE 4: INTEGRATION TESTING (Days 16-18)

**Duration**: 3 working days

**Activities**:
- Execute end-to-end scenarios (BB-E2E series)
- Execute integration tests (WB-INT series)
- Validate state consistency across components
- Test multi-component workflows
- Cross-component defect identification
- Fix integration defects
- Retest integration scenarios
- Performance spot checks (informal)

**Test Cases Executed**:
- End-to-End Scenarios: 5 tests (BB-E2E series)
- Integration Tests: 10 tests (WB-INT series)
- **Total**: 15 integration tests

**Deliverables**:
- Integration test execution report
- End-to-end scenario results
- State consistency validation report
- Integration defect log

**Entry Criteria**:
- Functional testing phase complete
- All P0/P1 functional defects resolved
- Application stable for multi-step scenarios

**Exit Criteria**:
- All integration scenarios pass
- All E2E workflows complete successfully
- No critical integration defects open
- State consistency verified

**Milestones**:
- Day 16: E2E scenarios executed
- Day 17: Integration tests complete
- Day 18: All integration defects fixed

**Resources**:
- Manual Testers: 12 hours each
- Automation Engineers: 12 hours each
- Developers: 8 hours each
- Test Lead: 6 hours
- Total: 62 person-hours

**Risks**: See Section 10.3 (R-01)

--------------------------------------------------------------------------------

#### PHASE 5: REGRESSION & TEST CLOSURE (Days 19-20)

**Duration**: 2 working days

**Activities**:
- Execute full regression suite (all P0 + P1 tests)
- Verify all defect fixes
- Final smoke test
- Generate test summary report
- Collect test metrics
- Calculate coverage statistics
- Create lessons learned document
- Archive test artifacts
- Test sign-off meeting

**Deliverables**:
- **Final Test Summary Report** including:
  - Total test cases executed
  - Pass/Fail statistics
  - Defect summary by severity
  - Code coverage metrics
  - Test execution timeline
  - Known issues and limitations
- **Lessons Learned Document**
- **Test Metrics Report**
- **Test Closure Sign-off**

**Entry Criteria**:
- Integration testing complete
- All P0 defects resolved
- All P1 defects resolved or accepted as known issues

**Exit Criteria**:
- 95%+ of all P0 + P1 tests passing
- Zero critical (S1) defects open
- Test summary report approved
- Stakeholder sign-off obtained

**Milestones**:
- Day 19: Regression suite complete
- Day 20: Test summary report delivered, sign-off obtained

**Resources**:
- Manual Testers: 8 hours each
- Test Lead: 12 hours
- Total: 28 person-hours

**Risks**: See Section 10.3 (R-06)

--------------------------------------------------------------------------------

### 9.3 MILESTONE SUMMARY

| Milestone | Target Date | Deliverable |
|-----------|-------------|-------------|
| M1: Test Environment Ready | Day 5 | Environments configured, test data created |
| M2: Unit Testing Complete | Day 10 | 90% code coverage achieved |
| M3: Functional Testing Complete | Day 15 | All P0/P1 functional tests executed |
| M4: Integration Testing Complete | Day 18 | All integration scenarios pass |
| M5: Test Closure | Day 20 | Test summary report approved |

### 9.4 DEPENDENCIES AND CRITICAL PATH

**CRITICAL PATH**: Phase 1 → Phase 2 → Phase 3 → Phase 4 → Phase 5

**KEY DEPENDENCIES**:
1. Phase 2 cannot start until Phase 1 complete (environment dependency)
2. Phase 3 cannot start until 90% code coverage achieved in Phase 2
3. Phase 4 cannot start until all P0 functional defects resolved
4. Phase 5 requires all previous phases complete

**PARALLEL ACTIVITIES**:
- Developers can fix defects while testing continues (with coordination)
- Test data creation can happen during environment setup
- Documentation can be updated throughout testing

### 9.5 CONTINGENCY BUFFER

**Built-in Slack**: 2 days (included in 20-day timeline)
- Day 10: 1-day buffer after unit testing
- Day 15: 1-day buffer after functional testing

**If Schedule Slips**:
- Prioritize P0 tests only
- Reduce P2 test coverage
- Extend timeline by 2-3 days maximum
- See Risk R-08 (Timeline Slippage) mitigation

================================================================================

## 10. RISK MANAGEMENT

### 10.1 RISK IDENTIFICATION

This section identifies potential risks to the testing effort, assesses their probability and impact, and defines mitigation and contingency strategies.

**RISK SCORING**:
- **Probability**: Low (1), Medium (2), High (3)
- **Impact**: Low (1), Medium (2), High (3)
- **Risk Score**: Probability × Impact (1-9)

**PRIORITIZATION**:
- High Priority (Score 6-9): Requires immediate mitigation planning
- Medium Priority (Score 3-4): Monitor closely, mitigate as needed
- Low Priority (Score 1-2): Monitor periodically

--------------------------------------------------------------------------------

### 10.2 RISK ASSESSMENT MATRIX

| Risk ID | Risk Description | Probability | Impact | Score | Priority |
|---------|------------------|-------------|--------|-------|----------|
| R-01 | Inadequate test coverage | Medium (2) | High (3) | 6 | HIGH |
| R-02 | Environment setup delays | Low (1) | Medium (2) | 2 | LOW |
| R-03 | Late requirements changes | Medium (2) | High (3) | 6 | HIGH |
| R-04 | Insufficient test resources | Low (1) | High (3) | 3 | MEDIUM |
| R-05 | Automation framework issues | Medium (2) | Medium (2) | 4 | MEDIUM |
| R-06 | Defect resolution delays | High (3) | Medium (2) | 6 | HIGH |
| R-07 | Test data issues | Low (1) | Medium (2) | 2 | LOW |
| R-08 | Timeline slippage | Medium (2) | Medium (2) | 4 | MEDIUM |

--------------------------------------------------------------------------------

### 10.3 DETAILED RISK ANALYSIS AND MITIGATION

#### RISK R-01: Inadequate Test Coverage

**Description**: Test cases may not cover all critical functionality or edge cases, leading to undetected defects in production.

**Probability**: Medium (2)  
**Impact**: High (3)  
**Risk Score**: 6 (HIGH PRIORITY)

**Triggers**:
- Code coverage below 90% target
- Missing test cases for new features
- Complex code paths not tested

**Mitigation Strategies**:
1. **Use code coverage tools** (Coverlet, dotCover) to identify untested code
2. **Conduct code reviews** to identify missing test scenarios
3. **Track coverage metrics daily** during Phase 2 (Unit Testing)
4. **Peer review test cases** before execution
5. **Create traceability matrix** linking requirements to test cases

**Contingency Plans**:
- If coverage < 90%: Extend Phase 2 by 2 days to add tests
- If gaps found late: Add high-priority tests to regression suite
- If time runs short: Focus on P0 critical path coverage only

**Monitoring**:
- Daily coverage reports during Phase 2
- Weekly gap analysis meetings
- Track: % coverage per class, untested branches

--------------------------------------------------------------------------------

#### RISK R-02: Environment Setup Delays

**Description**: Delays in setting up test environments, installing tools, or configuring systems could push back test start date.

**Probability**: Low (1)  
**Impact**: Medium (2)  
**Risk Score**: 2 (LOW PRIORITY)

**Triggers**:
- Tool installation failures
- Network/access issues
- Hardware problems

**Mitigation Strategies**:
1. **Prepare environment checklist** in advance (before Day 1)
2. **Test tool installations** on one machine first before rolling out
3. **Have IT support on standby** during setup week
4. **Download all required software** ahead of time
5. **Document setup procedures** for repeatability

**Contingency Plans**:
- Use backup machines if primary workstations fail
- Start with 2 environments if 4 cannot be ready
- Cloud-based development environment as backup option

**Monitoring**:
- Track setup progress against checklist
- Daily standup during Phase 1

--------------------------------------------------------------------------------

#### RISK R-03: Late Requirements Changes

**Description**: Changes to requirements during testing phase could invalidate test cases, require rework, and delay completion.

**Probability**: Medium (2)  
**Impact**: High (3)  
**Risk Score**: 6 (HIGH PRIORITY)

**Triggers**:
- Stakeholder requests new features
- Business priority shifts
- Misunderstood requirements discovered

**Mitigation Strategies**:
1. **Freeze requirements** before Phase 1 begins
2. **Obtain stakeholder sign-off** on SRS document
3. **Implement change control process**:
   - All changes require approval
   - Impact analysis before accepting
   - Re-estimate timeline if changes accepted
4. **Prioritize new requirements** for next release
5. **Conduct requirements review** at project kickoff

**Contingency Plans**:
- If minor change: Update affected tests only, add 1-2 days
- If major change: Re-scope testing, remove P2 tests, extend timeline
- If late in cycle: Defer changes to next release

**Monitoring**:
- Weekly requirements stability check
- Track change requests and their impact
- Red flag any changes during Phase 3-5

--------------------------------------------------------------------------------

#### RISK R-04: Insufficient Test Resources

**Description**: Not enough testers, engineers, or time allocated could result in incomplete testing or missed defects.

**Probability**: Low (1)  
**Impact**: High (3)  
**Risk Score**: 3 (MEDIUM PRIORITY)

**Triggers**:
- Team member unavailability (sick, vacation, reassigned)
- Underestimated testing effort
- Competing project priorities

**Mitigation Strategies**:
1. **Allocate dedicated resources** (7 people for 4 weeks)
2. **Train backup testers** who can step in if needed
3. **Prioritize test cases** (P0 → P1 → P2) to ensure critical tests done first
4. **Automate repetitive tests** to save manual effort
5. **Cross-train team members** for flexibility

**Contingency Plans**:
- If person leaves: Redistribute work, extend timeline by 2-3 days
- If multiple absences: Focus on P0 tests only, skip P2
- If timeline too short: Request 1-week extension with management approval

**Monitoring**:
- Track resource availability daily
- Monitor burn rate vs. planned effort
- Weekly resource utilization review

--------------------------------------------------------------------------------

#### RISK R-05: Automation Framework Issues

**Description**: Problems with test automation tools, frameworks, or infrastructure could slow or block automated testing.

**Probability**: Medium (2)  
**Impact**: Medium (2)  
**Risk Score**: 4 (MEDIUM PRIORITY)

**Triggers**:
- MSTest/NUnit framework bugs
- Code coverage tool failures
- Build system issues
- Incompatibility problems

**Mitigation Strategies**:
1. **Test framework early** during Phase 1 setup
2. **Have alternative tools ready**:
   - MSTest OR NUnit (not just one)
   - Coverlet OR dotCover
3. **Create simple "hello world" test** to validate setup
4. **Keep frameworks updated** but use stable versions
5. **Document known issues** and workarounds

**Contingency Plans**:
- If tool fails: Switch to backup tool (add 1 day for reconfiguration)
- If automation blocked: Fall back to manual code review + limited automation
- If persistent issues: Manual testing for critical paths, defer automation

**Monitoring**:
- Daily build and test execution status
- Track automation success rate
- Investigate any test framework errors immediately

--------------------------------------------------------------------------------

#### RISK R-06: Defect Resolution Delays

**Description**: Developers may not fix defects quickly enough, blocking test progress and preventing test closure.

**Probability**: High (3)  
**Impact**: Medium (2)  
**Risk Score**: 6 (HIGH PRIORITY)

**Triggers**:
- Complex defects requiring investigation
- Developer workload conflicts
- Unclear defect descriptions
- Low defect priority by developers

**Mitigation Strategies**:
1. **Hold daily defect triage meetings** during Phase 3-5
2. **Allocate dedicated developer time** (2 developers, 5-20 hours/week)
3. **Write clear, reproducible defect reports** with:
   - Steps to reproduce
   - Expected vs. actual results
   - Screenshots/logs
   - Suggested priority
4. **Escalate P0 defects immediately** to test lead and developers
5. **Track defect aging** (flag defects open > 2 days)

**Contingency Plans**:
- If P0 defect not fixed in 1 day: Escalate to management
- If multiple P1 defects blocked: Work on P2 tests while waiting
- If defect fixes delayed: Extend Phase 3 by 1-2 days, compress Phase 5

**Monitoring**:
- Daily defect status dashboard
- Track: Open vs. closed count, average resolution time
- Red flag: P0 defect open > 1 day, P1 defect open > 3 days

--------------------------------------------------------------------------------

#### RISK R-07: Test Data Issues

**Description**: Problems with test data files (missing, corrupted, incorrect format) could block or invalidate test execution.

**Probability**: Low (1)  
**Impact**: Medium (2)  
**Risk Score**: 2 (LOW PRIORITY)

**Triggers**:
- Test data files not created
- Incorrect test data format
- File corruption or access issues
- Test data deleted accidentally

**Mitigation Strategies**:
1. **Create all test data files early** (Phase 1, Days 1-5)
2. **Validate test data files** before using in tests
3. **Version control test data** (store in Git repository)
4. **Document test data format** clearly in Section 6
5. **Back up test data files** to multiple locations

**Contingency Plans**:
- If file missing: Regenerate from documented requirements (< 30 min)
- If file corrupted: Restore from version control or backup
- If format wrong: Update file and re-run affected tests (add 1-2 hours)

**Monitoring**:
- Verify test data files exist before each test phase
- Validate file format in automated setup script
- Track any test data issues in defect log

--------------------------------------------------------------------------------

#### RISK R-08: Timeline Slippage

**Description**: Testing phases may take longer than planned, risking the overall project timeline and release date.

**Probability**: Medium (2)  
**Impact**: Medium (2)  
**Risk Score**: 4 (MEDIUM PRIORITY)

**Triggers**:
- Higher defect count than expected
- Slower test execution than planned
- Resource availability issues
- Cumulative delays from other risks

**Mitigation Strategies**:
1. **Build in contingency buffer** (2 days included in 20-day plan)
2. **Track progress daily** against planned timeline
3. **Identify delays early** (red flag if > 1 day behind)
4. **Prioritize test execution** (P0 first, P1 second, P2 last)
5. **Be prepared to cut scope** (skip P2 tests if needed)

**Contingency Plans**:
- If 1-2 days behind: Use contingency buffer, work extra hours if needed
- If 3-4 days behind: Cut P2 test execution, focus on P0 + P1 only
- If > 5 days behind: Request formal timeline extension, reduce test scope significantly

**Monitoring**:
- Daily: Compare actual vs. planned test execution
- Track: Tests executed vs. planned, tests remaining, days left
- Weekly: Update timeline forecast, identify trends

**Escalation Triggers**:
- 2 days behind schedule → Notify management
- 3 days behind → Scope reduction discussion
- 5 days behind → Formal timeline extension request

--------------------------------------------------------------------------------

### 10.4 RISK MONITORING AND REPORTING

**MONITORING FREQUENCY**:
- Daily: Track active High Priority risks (R-01, R-03, R-06)
- Weekly: Review all risks in team meeting
- Ad-hoc: Monitor when specific triggers occur

**REPORTING**:
- **Daily Standup**: Quick status of high-priority risks
- **Weekly Status Report**: Risk dashboard included
- **Risk Register**: Maintained by Test Lead, updated continuously

**RISK DASHBOARD METRICS**:
- Number of open risks by priority
- Risks with active mitigation in progress
- Risks that have materialized (became issues)
- Trend: Risks increasing or decreasing

**ESCALATION PROCESS**:
1. Test Lead identifies risk materializing
2. Assess impact on timeline/quality
3. Activate contingency plan
4. If contingency insufficient → Escalate to management
5. Document outcome and lessons learned

--------------------------------------------------------------------------------

### 10.5 RISK RESPONSE SUMMARY

| Phase | High-Priority Risks | Mitigation Actions |
|-------|---------------------|-------------------|
| Phase 1 (Prep) | R-02 | Environment checklist, early tool validation |
| Phase 2 (Unit) | R-01, R-05 | Daily coverage tracking, test framework validation |
| Phase 3 (Functional) | R-03, R-06 | Requirements freeze, daily defect triage |
| Phase 4 (Integration) | R-01 | Integration test review, gap analysis |
| Phase 5 (Closure) | R-06, R-08 | Final regression prioritization, timeline monitoring |

================================================================================

## 11. SUCCESS CRITERIA

### 11.1 BLACK-BOX SUCCESS CRITERIA

**TEST EXECUTION**:
- ✅ 95%+ of planned black-box tests executed
- ✅ 100% of P0 (Critical) tests executed
- ✅ 95%+ of P1 (High) tests executed

**TEST RESULTS**:
- ✅ 100% of P0 tests passing
- ✅ 95%+ of P1 tests passing
- ✅ 90%+ of P2 tests passing

**DEFECT STATUS**:
- ✅ Zero open P0 (Critical) defects
- ✅ Zero open P1 (High) defects (or accepted as known issues)
- ✅ P2 defects documented for future release

**FUNCTIONAL VALIDATION**:
- ✅ All positive test cases pass
- ✅ All negative test cases produce correct errors
- ✅ No unexpected behaviors in workflows
- ✅ User experience is consistent with requirements

--------------------------------------------------------------------------------

### 11.2 WHITE-BOX SUCCESS CRITERIA

**CODE COVERAGE**:
- ✅ 95%+ line coverage for PhoneFileLoader class
- ✅ 95%+ line coverage for PhoneSystem class
- ✅ 90%+ line coverage for CommandProcessor class
- ✅ 80%+ line coverage for UserInterface class
- ✅ 75%+ line coverage for Program class
- ✅ 90%+ overall project code coverage

**BRANCH COVERAGE**:
- ✅ 90%+ branch coverage for all critical methods
- ✅ 100% of validation branches tested
- ✅ 100% of error handling branches tested

**TEST EXECUTION**:
- ✅ 100% of white-box unit tests execute successfully
- ✅ All critical paths verified
- ✅ All boundary conditions tested

**CODE QUALITY**:
- ✅ No null reference exceptions in tested code
- ✅ State consistency maintained across all operations
- ✅ No memory leaks detected
- ✅ No unreachable code in critical paths

--------------------------------------------------------------------------------

### 11.3 INTEGRATION SUCCESS CRITERIA

**END-TO-END SCENARIOS**:
- ✅ All 5 E2E scenarios (BB-E2E-001 through BB-E2E-005) pass
- ✅ Multi-step workflows complete without errors
- ✅ State remains consistent across components

**INTEGRATION TESTS**:
- ✅ All 10 integration tests (WB-INT series) pass
- ✅ Cross-component interactions validated
- ✅ Data structure integrity maintained

**SYSTEM STABILITY**:
- ✅ No crashes during extended test sessions
- ✅ Graceful error handling for all invalid inputs
- ✅ Memory stable over multiple operations

--------------------------------------------------------------------------------

### 11.4 TIMELINE SUCCESS CRITERIA

- ✅ Testing completed within 4-week timeline (20 days)
- ✅ All 5 phases completed on schedule (±2 days acceptable)
- ✅ All milestones achieved
- ✅ Test summary report delivered by Day 20

--------------------------------------------------------------------------------

### 11.5 DOCUMENTATION SUCCESS CRITERIA

**DELIVERABLES COMPLETE**:
- ✅ Test summary report created and approved
- ✅ Code coverage report generated
- ✅ Defect log complete and up-to-date
- ✅ Test metrics collected and analyzed
- ✅ Lessons learned documented

**TRACEABILITY**:
- ✅ All requirements have corresponding test cases
- ✅ All test cases traceable to requirements
- ✅ All defects linked to test cases

--------------------------------------------------------------------------------

### 11.6 STAKEHOLDER ACCEPTANCE

**SIGN-OFF OBTAINED**:
- ✅ Test Lead approves test execution results
- ✅ Development Lead accepts defect resolution
- ✅ Project Manager approves test closure
- ✅ Stakeholders accept known limitations

**QUALITY GATES PASSED**:
- ✅ All entry criteria met for each phase
- ✅ All exit criteria met for each phase
- ✅ No critical quality issues remaining

--------------------------------------------------------------------------------

### 11.7 OVERALL PROJECT SUCCESS DEFINITION

**The testing effort is considered successful when**:
1. ✅ All P0 and P1 test cases pass (100% and 95% respectively)
2. ✅ 90%+ code coverage achieved
3. ✅ Zero critical defects remain open
4. ✅ Testing completed within timeline (20 days ±2)
5. ✅ Test summary report delivered and approved
6. ✅ Stakeholder sign-off obtained
7. ✅ Application meets all requirements from SRS document

**Minimum Acceptable Criteria** (if scope must be reduced):
- 100% of P0 tests pass
- 90% P1 tests pass
- 85% code coverage
- Zero P0 defects open
- Testing within 22 days

================================================================================

## 12. TEST REPORTING

### 12.1 DAILY STATUS REPORT

**Frequency**: Daily during testing phases (Days 6-20)  
**Audience**: Test team, developers, test lead  
**Format**: Email or dashboard update

**Contents**:
- Tests executed today vs. planned
- Tests passed / failed / blocked
- New defects found (by severity)
- Defects fixed and retested
- Current blockers or risks
- Plan for tomorrow

--------------------------------------------------------------------------------

### 12.2 WEEKLY STATUS REPORT

**Frequency**: Weekly (end of week)  
**Audience**: Test lead, project manager, stakeholders  
**Format**: Formal report document

**Contents**:
- Overall progress (% tests complete)
- Test execution metrics
- Defect metrics (opened, closed, open by severity)
- Code coverage progress
- Risks and issues
- Upcoming week plan
- Red/yellow/green status indicator

--------------------------------------------------------------------------------

### 12.3 PHASE COMPLETION REPORT

**Frequency**: End of each phase (Days 5, 10, 15, 18, 20)  
**Audience**: All stakeholders  
**Format**: Formal report with sign-off

**Contents**:
- Phase objectives and deliverables
- Test execution summary
- Exit criteria verification
- Defect summary
- Risks and issues
- Readiness for next phase
- Sign-off approval

--------------------------------------------------------------------------------

### 12.4 FINAL TEST SUMMARY REPORT

**Frequency**: Once (Day 20)  
**Audience**: All project stakeholders  
**Format**: Comprehensive formal report

**Contents**:
1. **Executive Summary**
   - Overall test results
   - Pass/fail statistics
   - Critical findings
   - Quality assessment

2. **Test Execution Summary**
   - Total test cases planned vs. executed
   - Black-box test results
   - White-box test results
   - Integration test results

3. **Code Coverage Analysis**
   - Coverage by class/method
   - Coverage trends
   - Untested areas (if any)

4. **Defect Analysis**
   - Total defects by severity
   - Defects by component
   - Defect resolution time
   - Open defects (known issues)

5. **Test Metrics**
   - Test execution rate
   - Defect detection rate
   - Test effectiveness
   - Coverage metrics

6. **Timeline Analysis**
   - Planned vs. actual schedule
   - Delays and causes
   - Timeline recommendations

7. **Risks and Issues**
   - Risks that materialized
   - How they were handled
   - Outstanding risks

8. **Quality Assessment**
   - Overall quality rating
   - Readiness for release
   - Known limitations

9. **Recommendations**
   - Future testing improvements
   - Process enhancements
   - Tool recommendations

10. **Lessons Learned**
    - What went well
    - What could be improved
    - Best practices identified

11. **Appendices**
    - Detailed test results
    - Code coverage reports
    - Defect list
    - Test data files used

--------------------------------------------------------------------------------

### 12.5 METRICS TRACKED

**TEST EXECUTION METRICS**:
- Total test cases planned
- Total test cases executed
- Pass / Fail / Blocked / Not Run counts
- Test execution rate (tests per day)
- Test effectiveness (defects found per test)

**DEFECT METRICS**:
- Defects opened by day
- Defects closed by day
- Open defects by severity
- Average defect resolution time
- Defect density (defects per 1000 LOC)
- Defect detection rate by phase

**COVERAGE METRICS**:
- Line coverage percentage
- Branch coverage percentage
- Method coverage percentage
- Class coverage percentage
- Untested code (LOC)

**QUALITY METRICS**:
- Test pass rate
- Requirements coverage
- Code coverage
- Defect escape rate (to later phases)
- Test case effectiveness

**TIMELINE METRICS**:
- Planned vs. actual days per phase
- Schedule variance
- Critical path delays
- Resource utilization

**RISK METRICS**:
- Number of risks identified
- Number of risks materialized
- Risk mitigation effectiveness

--------------------------------------------------------------------------------

### 12.6 DEFECT REPORTING TEMPLATE

**Defect ID**: [Auto-generated]  
**Title**: [Short description]  
**Reporter**: [Name]  
**Date Reported**: [Date]

**Severity**:
- S1 (Critical): Application crash, data corruption
- S2 (High): Major functionality broken
- S3 (Medium): Minor functionality issues
- S4 (Low): Cosmetic issues

**Priority**:
- P0: Must fix before release
- P1: Should fix before release
- P2: Can defer to next release

**Component**: [PhoneFileLoader / PhoneSystem / CommandProcessor / etc.]

**Test Case ID**: [Reference to test case that found defect]

**Description**: [Detailed description]

**Steps to Reproduce**:
1. [Step 1]
2. [Step 2]
3. [Step 3]

**Expected Result**: [What should happen]

**Actual Result**: [What actually happens]

**Attachments**: [Screenshots, logs, test data]

**Environment**: [OS, .NET version, build number]

**Status**: Open / In Progress / Fixed / Verified / Closed

**Assigned To**: [Developer name]

**Resolution**: [How it was fixed]

**Verification**: [Retest results]

================================================================================

## 13. APPENDIX

### 13.1 TEST CASE PRIORITIES SUMMARY

**P0 (Critical) - 87 test cases**:
- Essential functionality that must work
- Application startup and basic operations
- Core call management features
- Executed first in every test phase

**P1 (High) - 124 test cases**:
- Important functionality
- Error handling and edge cases
- Secondary features
- Executed after P0 tests pass

**P2 (Low) - 39 test cases**:
- Nice-to-have features
- Optional functionality
- Cosmetic issues
- Executed if time permits

**Total Test Cases**: 250

### 13.2 ACRONYMS AND DEFINITIONS

- **BB**: Black-Box (testing type)
- **WB**: White-Box (testing type)
- **P0/P1/P2**: Priority levels (Critical/High/Low)
- **S1-S4**: Severity levels (Critical/High/Medium/Low)
- **LOC**: Lines of Code
- **E2E**: End-to-End
- **SRS**: Software Requirements Specification
- **TSS**: Telephone Switching System
