# Project Proposal: Medicine Shop Application Development

## Introduction

- **Platform: Leaning with [Tapos](https://www.linkedin.com/in/taposg/)**
- **Course Title: ASP.NET Core 8**
- **Project-01: Medicine Shop Application Development**

## Overview

This project aims to develop a simplified medicine shop application similar to [Arogga](https://www.arogga.com/). The application will focus primarily on the core functionalities of buying and selling medicines. It will cater to users looking to search for medicines based on their name or generic name, add selected medicines to a cart, and complete transactions seamlessly. The application will also allow users to search for products by categories like eye care, diabetes, pressure management, ENT, dental, etc. Upon completing transactions, users will receive invoices and delivery information.

## Objectives

1. **Develop an intuitive search functionality** to allow users to search for medicines using the medicine name, generic name, or category.
2. **Display detailed product information** including images, descriptions, prices, and generic names.
3. **Implement a cart feature** for users to add desired products for purchase.
4. **Facilitate seamless transactions** allowing users to pay for their selected products.
5. **Generate invoices and delivery information** for users upon successful payment.

## Features to Implement

1. **Search Functionality:**

   - Users can search for medicines using the medicine name or generic name.
   - The application will also allow searches based on product categories such as eye care, diabetes, pressure, ENT, dental, etc.

2. **Search Results Display:**

   - Display a list of products that match the search criteria.
   - Each product in the list will show essential details such as product name, generic name, and price.

3. **Product Details:**

   - Upon clicking a product, users will be presented with detailed information including:
     - Product image
     - Product name
     - Description
     - Price
     - Generic name

4. **Cart Functionality:**

   - Users can add selected products to a shopping cart.
   - The cart will display all selected products, their quantities, and the total price.

5. **Transaction Completion:**

   - Users can proceed to checkout, where they will review their cart and enter payment details.
   - After confirming the payment, the transaction will be completed.

6. **Invoice and Delivery Information:**
   - Upon successful transaction, users will receive an invoice.
   - Delivery information will be provided, outlining when and how the product will be delivered.
   
## Acknowledgments

I want to give special thanks to [Biswanath Ghosh Tapos](https://www.linkedin.com/in/taposg/) for his guidance and project assignments, which helped deepen my understanding of ASP.NET Core 8.

# Initial Assignments for the Medicine Shop Project
- Here is the [ER-Diagram](https://www.canva.com/design/DAGPtmQ7dOE/DziiDsVTh3prKyJB7xIAtw/edit?utm_content=DAGPtmQ7dOE&utm_campaign=designshare&utm_medium=link2&utm_source=sharebutton).
- It's editable, so feel free to add your observations or suggestions. I would greatly appreciate your input and support.

# How to Run the Medicine Application

### 1. Build and Migrate
- First, build the application.
- Then, run the migration.

```bash
dotnet ef migrations add InitialCreate --project MedicineShopApplication.DLL --startup-project MedicineShopApplication.API
```
```bash
dotnet ef database update --project MedicineShopApplication.DLL --startup-project MedicineShopApplication.API
```

### 2. Run the Application
- After running the application, it will automatically create the default application roles, including `UserRole`.
- Create a Dockerfile for ASP.NET Core with Redis installation inside the `aspnet-docker` container.

### 3. Postman Collection
- You can access the Postman collection for this project using the link below:
- [View Postman Collection](https://learning-dotnet.postman.co/workspace/Learning-dotnet-Workspace~7b5f8c1a-55c1-4ff4-adee-eba137d6bcea/collection/38564146-caf1e641-712b-4e41-9ce9-92f48bd57009?action=share&creator=38564146&active-environment=38564146-6e7a4d1c-b57e-4cbb-9c75-75e3d81813b4)

### 4. Register the User
- Register a user by hitting the following endpoint: `{{server}}/Account/register`.
- By default, this will create an admin user with the role name `developer`.

### 5. Obtain an Access Token
- Use Postman to hit the following endpoint: `{{server}}/connect/token` with the following parameters:
  - `username: 01670047320`
  - `password: 12345`
  - `clientId: msa-web-dev`
  - `clientSecret: 901564A5-E7FE-42CB-W10D-61EF6A8F3654`
  - `grant_type: password`
  
Once successfully logged in, proceed with creating the initial data.

### 6. Create Initial Data
- **Create Brands:**  
  Hit the endpoint: `{{server}}/Brand/create-range`
  
- **Create Categories:**  
  Hit the endpoint: `{{server}}/Category/create-range`
  
- **Create Unit of Measures:**  
  Hit the endpoint: `{{server}}/UnitOfMeasure/create-range`
  
- **Create Products:**  
  Hit the endpoint: `{{server}}/Product/create-range`
  
- **Create Customers:**  
  Hit the endpoint: `{{server}}/Customer/create`


# Medicine Application - Queries and Topics

## What are some topic concepts that we missed?

- **File/Image Upload**  
  Implement a reliable mechanism for file or image uploads, handling validation and storage effectively.

- **Configuring centralized logging in ASP.NET Core**  
  Set up centralized logging using a framework like Serilog or NLog to ensure all logs are captured and managed consistently across the application.

- **Issue with OAuth2 token generation**  
  There is an issue where, despite using different applications, changing parameters and accessing the OAuth2 token endpoint still generates a token.  
  **Expected behavior:** Any mismatch in client-specific parameters should prevent token issuance and return an error (e.g., `invalid_client`).

---

## Some queries while developing the Medicine Application

### Differentiating between Admin and Customer Users

- In a small or single application, how do you differentiate between an Admin user and a Customer user?  
  We know about the `AspNetUserRole` table, but accessing it with a custom `ApplicationUser` table is complex. Although I've managed it, I feel it's not the ideal or best practice.

### Role-Based Authorization in API Endpoints

- What is the proper way to implement role-based authorization for API endpoints?

### Soft Delete with Undo

- How can we implement soft delete with the ability to undo it?

### Managing Auditable Entities

- What is the best practice for managing auditable entities, and how can we efficiently return `CreatedByName` and `UpdatedByName`?  
  I’ve managed it, but I believe the current approach is not ideal in terms of performance.

### Implementing Stripe Payments

- How should Stripe be implemented for payments in this application?

### Unique Code Generation for Entities

- In the application, I have entities like `Category` and `Product`, and I'm trying to generate unique codes at the application level.  
  - What is the best practice: should these codes be input by the user or generated at the application level?  
  - If they should be generated by the application, what is the best method to ensure uniqueness?

### NULL vs. Nullable

- Though it’s a simple concept, I get confused sometimes: how to dealing with Nullable`?

### Efficient Data Access

- What is the best technique to avoid repeatedly hitting the database (e.g., within a `foreach` loop) when fetching complex or bulk data?