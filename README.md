# Clinic Management System (C#)

A **C# Windows Forms application** for managing clinic operations efficiently. Handles **patients, appointments, and medical records** with secure role-based access.

---

## **Description**
This project provides a fully functional Clinic Management System with modules for **Admin, Doctor, Patient, and Receptionist**.  
It allows for adding, updating, and viewing **patient records**, managing appointments, and maintaining contact and medical information.

---

## **Key Features**
- **Role-based access:** Admin, Doctor, Patient, Receptionist
- **Patient Management:** Add, update, delete patients
- **Appointment Management:** Schedule, update, and track appointments
- **Medical Info:** Record CNIC, age, allergies, and medical history
- **Data Validation:** Ensures correct phone numbers, CNIC, and age inputs
- **Auto-calculated age** from date of birth
- **Dynamic form placeholders** for improved UX

---

## **Modules**
1. **Patient Module:** Add, edit, delete patient details, including contact and medical info
2. **Doctor Module:** View assigned patients and manage availability
3. **Receptionist Module:** Handle appointment bookings and patient assignment
4. **Admin Module:** Manage users, doctors, and system settings

---

## **Installation**
1. Clone the repository:  
   ```bash
   git clone https://github.com/Muzamil-Fatima/medicare-system.git

2. Open the solution in **Visual Studio**
3. Update the **connection string** in `ValidationHelper.ConnectionString` to match your SQL Server setup
4. Build and run the project

---

## **Database**

* **SQL Server** database with tables:

  * `tblPatient`, `tblPatientContact`, `tblPatientMedical`, `tblAppointments`, `tblDoctorAvailability`
* **Supports CRUD operations** for all patient and appointment records
* Transactions used for safe insertion of patient, contact, and medical data

---

## **Validation**

* Phone number: 10–15 digits
* CNIC: Must follow `12345-1234567-1` format
* Age: Positive integer only
* Gender: Selected from combo box

---

## **Screenshots**

* **Login Page**
* **Dashboard**
* **Patient Management Form**
* **Appointment Booking Form**
  *(Add screenshots to showcase your UI)*

---

## **Future Improvements**

* Add **email/SMS notifications** for appointments
* Dashboard analytics for doctors and admins
* Enhance **UI/UX with modern themes**
* Full **encryption and password hashing** for users

---

## **Technologies**

* **C# (.NET Framework)** – Windows Forms
* **SQL Server** – Database
* **Visual Studio** – IDE

```
