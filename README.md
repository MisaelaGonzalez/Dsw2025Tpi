# 🛠️ Trabajo Práctico Integrador

### 📚 Desarrollo de Software - Backend

---

## 👥 Integrantes
- **Gonzalez Zurita Misaela Antonella** - Legajo: 53245  
- **Delgado Lara Naredla** - Legajo: 56375    

---

## ⚙️ Instrucciones para uso local

1. Abrir la consola de NuGet en Visual Studio.
2. Ejecutar el comando: `Update-Database`.
3. La base de datos estará lista para usar la aplicación.

---

## 📌 Endpoints principales

### 1️⃣ Crear producto
- **Método:** `POST`  
- **Ruta:** `/api/products`  
- **Descripción:** Crea un producto con los datos enviados en el cuerpo JSON.  
- **Respuestas:** `201 Created` | `400 Bad Request`

### 2️⃣ Listar productos
- **Método:** `GET`  
- **Ruta:** `/api/products`  
- **Descripción:** Devuelve todos los productos activos.  
- **Respuestas:** `200 OK` | `204 No Content`

### 3️⃣ Obtener producto por ID
- **Método:** `GET`  
- **Ruta:** `/api/products/{id}`  
- **Descripción:** Retorna los detalles del producto solicitado.  
- **Respuestas:** `200 OK` | `404 Not Found`

### 4️⃣ Actualizar producto
- **Método:** `PUT`  
- **Ruta:** `/api/products/{id}`  
- **Descripción:** Actualiza completamente un producto existente.  
- **Respuestas:** `200 OK` | `400 Bad Request` | `404 Not Found`

### 5️⃣ Inhabilitar producto
- **Método:** `PATCH`  
- **Ruta:** `/api/products/{id}`  
- **Descripción:** Cambia el estado del producto a inactivo.  
- **Respuestas:** `200 OK` | `404 Not Found`

### 6️⃣ Crear orden
- **Método:** `POST`  
- **Ruta:** `/api/orders`  
- **Descripción:** Registra una nueva orden. Verifica stock antes de crear.  
- **Respuestas:** `201 Created` | `400 Bad Request`

### 7️⃣ Listar órdenes
- **Método:** `GET`  
- **Ruta:** `/api/orders`  
- **Descripción:** Devuelve todas las órdenes con filtros opcionales (estado, cliente, paginación).  
- **Respuestas:** `200 OK` | `500 Internal Server Error`

### 8️⃣ Obtener orden por ID
- **Método:** `GET`  
- **Ruta:** `/api/orders/{id}`  
- **Descripción:** Muestra detalles completos de una orden específica.  
- **Respuestas:** `200 OK` | `404 Not Found`

### 9️⃣ Actualizar estado de orden
- **Método:** `PUT`  
- **Ruta:** `/api/orders/{id}/status`  
- **Descripción:** Cambia el estado de una orden (ej: Processing, Shipped).  
- **Respuestas:** `200 OK` | `400 Bad Request` | `404 Not Found`

---

📝 *Este backend fue desarrollado como parte del Trabajo Práctico Integrador de la materia Desarrollo de Software.*