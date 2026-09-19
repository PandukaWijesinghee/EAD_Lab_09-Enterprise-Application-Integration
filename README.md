# Lab 09 - Enterprise Application Integration (EAI)

> **Course:** SE4040 - Enterprise Application Development  
> **Topic:** Integrating Independent Applications using REST APIs  

---

## 📌 Overview

This project demonstrates a practical implementation of **Enterprise Application Integration (EAI)** using two independent ASP.NET Core Web API services. The integration demonstrates application-level integration via remote procedure invocation (HTTP REST) exchanging **JSON** payloads.

### Scenario
An online shopping company operates two separate applications:
1. **Order Management System (OMS)**: Receives incoming orders from customers and decides whether an order can be accepted.
2. **Inventory Management System (IMS)**: Manages product details and tracks real-time stock levels.

Before accepting any customer order, the **Order Management System** automatically queries the **Inventory Management System** REST API to verify product availability and stock quantity.

---

## 🏗️ Architecture & Interaction Flow

```
+----------+                     +---------------------------+                     +-------------------------------+
| Customer |  -- POST /orders -> |  Order Management System  |  -- GET /inventory ->|  Inventory Management System  |
| (Client) |  <- JSON Response - |      (Port: 5107)         |  <- JSON Response - |         (Port: 5044)          |
+----------+                     +---------------------------+                     +-------------------------------+
```

### Integration Workflow
1. Customer submits an order via `POST /api/orders`.
2. Order Management System receives the order payload.
3. Order Management System makes a synchronous HTTP `GET` call to the Inventory API: `http://localhost:5044/api/inventory/{productName}`.
4. Inventory Management System checks the product existence and available stock quantity.
5. Order Management System evaluates the inventory response:
   - If stock is sufficient (`available == true` and `quantity >= requestedQuantity`): Accepts order (`200 OK`).
   - If stock is insufficient: Rejects order (`400 Bad Request`).
   - If product is not found: Returns not found (`404 Not Found`).

---

## 🚀 Projects & Ports

| Service | Technology | HTTP Base URL | HTTPS Base URL | Swagger UI |
| :--- | :--- | :--- | :--- | :--- |
| **Inventory Management System** | ASP.NET Core 8 Web API | `http://localhost:5044` | `https://localhost:7037` | `/swagger` |
| **Order Management System** | ASP.NET Core 8 Web API | `http://localhost:5107` | `https://localhost:7187` | `/swagger` |

---

## 📡 API Specifications

### 1. Inventory Management System

#### Endpoint: Get Product Inventory
- **URL:** `GET /api/inventory/{productName}`
- **Route Parameter:** `productName` (string, case-insensitive)

**Sample Success Response (`200 OK`):**
```json
{
  "productName": "Laptop",
  "quantity": 15,
  "available": true
}
```

**Sample Error Response (`404 Not Found`):**
```json
{
  "message": "Product not found"
}
```

---

### 2. Order Management System

#### Endpoint: Place Customer Order
- **URL:** `POST /api/orders`
- **Content-Type:** `application/json`

**Sample Request Body:**
```json
{
  "orderId": 101,
  "customerName": "Nimal",
  "product": "Laptop",
  "quantity": 2
}
```

**Responses:**
- **Order Accepted (`200 OK`):**
  ```json
  {
    "message": "Order placed successfully"
  }
  ```
- **Insufficient Stock (`400 Bad Request`):**
  ```json
  {
    "message": "Insufficient stock"
  }
  ```
- **Product Not Found (`404 Not Found`):**
  ```json
  {
    "message": "Product not found"
  }
  ```
- **Service Unavailable (`500 Internal Server Error`):**
  ```json
  {
    "message": "Inventory service unavailable"
  }
  ```

---

## 🧪 Test Matrix

| Test Case | Product | Requested Quantity | Expected Result | Status Code |
| :--- | :--- | :---: | :--- | :---: |
| **TC-01** | `Laptop` | 2 | Order accepted | `200 OK` |
| **TC-02** | `Laptop` | 100 | Insufficient stock | `400 Bad Request` |
| **TC-03** | `UnknownProduct` | 1 | Product not found | `404 Not Found` |

---

## 💻 Running the Services Locally

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- API client such as Postman, curl, or Visual Studio `.http` files

### Step 1: Start Inventory Management System
Open a terminal in the project root:
```bash
cd InventoryManagementSystem
dotnet run --launch-profile http
```
The Inventory service will start at `http://localhost:5044`.

### Step 2: Start Order Management System
Open a second terminal in the project root:
```bash
cd OrderManagementSystem
dotnet run --launch-profile http
```
The Order service will start at `http://localhost:5107`.

---

## 🧠 EAI Analysis Summary

1. **EAI Approach:** Point-to-Point Integration via Remote Procedure Invocation / RESTful Web Services.
2. **Data Format:** JSON (JavaScript Object Notation).
3. **Synchronous vs. Asynchronous:** **Synchronous** — the Order Management System blocks and waits for a response from the Inventory Management System before responding to the customer.
4. **Availability Impact:** If the Inventory service goes down, the Order service cannot complete order validations and fails (`500 Internal Server Error`).
5. **Coupling:** The systems exhibit temporal coupling (both must be online simultaneously) and location/protocol coupling (Order Service depends on Inventory's host address and endpoint contract).
