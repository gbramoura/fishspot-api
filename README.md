# FishSpot API

The FishSpot API is built using C# and the .NET framework, which is a robust platform for developing high-performance applications. C# is a versatile, object-oriented programming language developed by Microsoft, widely used for building Windows applications, web services, and cloud-based applications. The .NET framework provides a comprehensive environment for developing and running applications with a rich library of classes and tools that simplify many programming tasks.

## Overview

The **FishSpot API** allows developers to create applications where users can register, share, and explore fishing spots. This community-driven platform helps anglers document their favorite fishing locations along with essential details about each spot.

## Features

* Spot Registration
* Spot Retrieval
* Spot Updates
* Spot Deletion
* User Authentication
* Location-Based Search

## Prerequisites

* .NET SDK installed on your machine
* Intermediary knowledge of C# and RESTful API principles
* MongoDB installed on your machine or a container
* Basic knowledge of MongoDB

## Installation

* **1. Clone the repository:**

```bash
git clone https://github.com/gbramoura/fishspot-api.git
cd fishspot-api
```

* **2. Restore dependencies:**

```bash
dotnet restore
```

* **3. Run the application:**:

```bash
dotnet run
```

The API will be available at http://localhost:5000.

## API Endpoints

### Auth Endpoint

| **Endpoint** | **Method** | **Description** | **Request Body**  | 
|---------|------------|--------------------|----------------------|
| `auth/register/`         | POST  | Registers a new user.                                                 | `{ "email": "user@example.com", "password": "securepassword123", "name": "John Doe" }`    | 
| `auth/login/`            | POST  | Login a user.                                                         | `{ "email": "user@example.com", "password": "securepassword123" }`                        | 
| `auth/refresh-token/`    | POST  | Refreshes the authentication token using a valid refresh token.       | `{ "refreshToken": "EXPIRED_REFRESH_TOKEN" }`                                             | 
| `auth/recover-password/` | POST  | Sends a password recovery token to the user's email.                  | `{ "email": "user@example.com" }`                                                         | 
| `auth/change-password/`  | POST  | Changes the user's password using a recovery token.                   | `{ "token": "VALID_RECOVERY_TOKEN", "newPassword": "newSecurePassword123" }`              | 

### Spot Endpoint

| **Endpoint** | **Method** | **Description**  | **Request Body** |
|--------------|------------|------------------|-------------------|
| `spot/`                            | POST       | Creates a new spot.                                             | `{ "id": "Spot Name", "location": [Latitude, Longitude], ... }`        | 
| `spot/{id}`                        | GET        | Retrieves a specific spot by its ID.                            | N/A                                                                    | 
| `spot/`                            | GET        | Retrieves a list of nearby locations.                           | N/A                                                                    | 
| `spot/by-user`                     | GET        | Retrieves spots associated with the authenticated user.         | `{ "page": 1, "size": 10 }`                                            | 
| `spot/{id}`                        | DELETE     | Deletes a specific spot by its ID.                              | N/A                                                                    |

### Resources Endpoint

| **Endpoint**                       | **Method** | **Description**                                                         | **Request Body**                                                        |
|------------------------------------|------------|-------------------------------------------------------------------------|-------------------------------------------------------------------------|
| `resources/{id}`                   | GET        | Retrieves a resource (e.g., image) by its ID.                           | N/A                                                                     |
| `resources/attach-to-spot`         | POST       | Attaches resources to a spot.                                           | `{ "spotId": "123", "files": [file1, file2] }` |
| `resources/dettach-to-spot`        | POST       | Detaches resources from a spot.                                         | `{ "spotId": "123", "files": [file1, file2] }` |

### Default Response
All endpoints follow the same structure for responses. Here's an example of response:

```json
{
  "code": 200,
  "message": "Brief description",
  "error": {},
  "response": {},
}
```
* **code**: HTTP status code.
* **message**: A human-readable message explaining the error.
* **error**: Optional field, containing detailed error information when necessary.
* **response**: Optional field, containing the expected response of the endpoint.

## Authentication

* All endpoints except for `register`, `login`, `recover-password`, and `refresh-token` require the user to be authenticated with a valid JWT token.
* The `Authorization` header should include the JWT token in the format:
```
Authorization: Bearer <token>
```


## Contributing

Contributions are welcome! Follow these steps to contribute:

* Fork the repository.
* Create a new branch: git checkout -b feature/YourFeature.
* Commit your changes: git commit -m 'Add some feature'.
* Push to the branch: git push origin feature/YourFeature.
* Open a pull request.

---

Thank you for using the FishSpot API! Happy fishing! 🎣
