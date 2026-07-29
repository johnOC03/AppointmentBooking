# Test Plan

## Project Information
- **Project**: Appointment Booking System
- **Version**: 1.0
- **Date**: July 2026
- **Prepared By**: Development Team

## 1. Introduction
This test plan describes the testing approach for the Appointment Booking System, including test objectives, scope, resources, schedule, and test deliverables.

## 2. Test Objectives
- Validate all business rules are implemented correctly
- Ensure data validation works as expected
- Verify error messages are clear and actionable
- Confirm system behavior matches requirements

## 3. Test Scope

### 3.1 Features to be Tested
1. **Doctor Management**
   - Doctor creation with validation
   - Available slots management
   - Maximum daily appointments limit
   - Slot reservation logic

2. **Patient Management**
   - Patient creation with validation
   - Patient ID format validation (P followed by digits)
   - Display name logic (preferred vs legal name)

3. **Appointment Request**
   - Request creation with validation
   - Date validation (no past dates)
   - Advance notice requirement (1 day)

4. **Appointment Booking Service**
   - Successful booking flow
   - Failure scenarios
   - Message generation
   - Slot and daily count management

### 3.2 Features Not to be Tested
- Database integration (not implemented)
- API endpoints (not implemented)
- User interface (not implemented)
- Performance under load

## 4. Test Approach

### 4.1 Unit Testing
- **Framework**: MSTest
- **Methodology**: White-box testing
- **Coverage**: All public methods and business logic

### 4.2 Test Design Techniques
- **Equivalence Partitioning**: Group similar inputs
- **Boundary Value Analysis**: Test edge cases
- **Error Guessing**: Test common failure scenarios
- **Decision Table Testing**: Test business rule combinations

## 5. Test Cases

### 5.1 Doctor Tests
| Test ID | Test Case | Expected Result |
|---------|-----------|-----------------|
| DOC-001 | Create doctor with valid data | Doctor created successfully |
| DOC-002 | Create doctor with empty ID | ArgumentException thrown |
| DOC-003 | Create doctor with negative slots | ArgumentException thrown |
| DOC-004 | Reserve slot when available | Slot count decrements |
| DOC-005 | Reserve slot when at daily limit | InvalidOperationException thrown |

### 5.2 Patient Tests
| Test ID | Test Case | Expected Result |
|---------|-----------|-----------------|
| PAT-001 | Create patient with valid ID (P001) | Patient created successfully |
| PAT-002 | Create patient with empty ID | ArgumentException thrown |
| PAT-003 | Create patient with invalid ID (A001) | ArgumentException thrown |
| PAT-004 | Create patient with invalid ID (P00A) | ArgumentException thrown |
| PAT-005 | Display name with preferred name | Returns preferred name |
| PAT-006 | Display name without preferred name | Returns legal name |

### 5.3 Appointment Request Tests
| Test ID | Test Case | Expected Result |
|---------|-----------|-----------------|
| REQ-001 | Create request for tomorrow | Request created successfully |
| REQ-002 | Create request for past date | ArgumentException thrown |
| REQ-003 | Create request for today (advance notice on) | ArgumentException thrown |
| REQ-004 | Create request for today (advance notice off) | Request created successfully |

### 5.4 Booking Service Tests
| Test ID | Test Case | Expected Result |
|---------|-----------|-----------------|
| SVC-001 | Book with available slots | Success=true, slot decremented |
| SVC-002 | Book with no available slots | Success=false, actionable message |
| SVC-003 | Book at daily limit | Success=false, actionable message |
| SVC-004 | Book with null request | Success=false, actionable message |
| SVC-005 | Multiple successful bookings | Slots decrement correctly |

## 6. Test Environment
- **IDE**: Visual Studio 2026
- **Framework**: .NET 8
- **Test Runner**: MSTest
- **Operating System**: Windows

## 7. Test Schedule
| Phase | Duration | Status |
|-------|----------|--------|
| Test Planning | 1 day | Complete |
| Test Design | 1 day | Complete |
| Test Execution | 1 day | In Progress |
| Test Reporting | 0.5 day | Pending |

## 8. Resources
- **Test Engineers**: 1
- **Developers**: 1
- **Tools**: Visual Studio, Git, MSTest

## 9. Test Deliverables
- [ ] Test Strategy Document
- [ ] Test Plan Document
- [ ] Test Cases (37 total)
- [ ] Test Summary Report
- [ ] Defect Reports (if applicable)

## 10. Entry and Exit Criteria

### Entry Criteria
- Requirements documented
- Code implementation complete
- Build passes without errors
- Test environment ready

### Exit Criteria
- All planned tests executed
- 100% test pass rate
- No open critical defects
- Code coverage ? 90%

## 11. Suspension and Resumption Criteria
### Suspension
- Critical defects blocking testing
- Test environment unavailable
- Build failures

### Resumption
- Defects fixed and verified
- Environment restored
- Successful build available

## 12. Risks and Contingencies
| Risk | Probability | Impact | Mitigation |
|------|-------------|--------|------------|
| Test environment issues | Low | Medium | Have backup environment |
| Requirement changes | Medium | High | Maintain flexible test design |
| Resource unavailability | Low | Medium | Cross-train team members |

## 13. Approvals
- **Test Lead**: _________________
- **Development Lead**: _________________
- **Project Manager**: _________________

---
*Document Version: 1.0*  
*Last Updated: July 2026*
