# 📸 Service_Image.api

## Description

**Service_Image.api** est une API backend avancée pour la gestion et la transformation d'images, inspirée de services comme Cloudinary.  
Cette API permet aux utilisateurs de **téléverser**, **transformer**, **récupérer** et **lister** des images de manière sécurisée et performante.

Ce projet démontre mes compétences en développement backend, gestion sécurisée d'authentification JWT, manipulation avancée d'images, et conception d'API RESTful modulaires.

---

## 🚀 Fonctionnalités principales

### 🔒 Authentification utilisateur

- **Inscription (Sign-Up)** : Créez un compte pour commencer à utiliser le service.
- **Connexion (Log-In)** : Connectez-vous pour accéder à vos images.
- **JWT** : Toutes les routes protégées nécessitent un token JWT valide.

---

### 🖼️ Gestion des images

- **Upload d'images** : Téléversez des images en utilisant `multipart/form-data`.
- **Transformations d'images** :
  - Redimensionnement (resize)
  - Recadrage (crop)
  - Rotation
  - Ajout de watermark
  - Miroir (mirror)
  - Renversement (flip)
  - Compression
  - Changement de format (JPEG, PNG, etc.)
  - Filtres (grayscale, sepia, etc.)
- **Récupération d'images** : Accédez à l'image originale ou transformée.
- **Listing des images** : Liste paginée des images téléversées par l'utilisateur, avec métadonnées.

---
## 💡 Captures de la documentation Swagger

### Vue d'ensemble de tous les endpoints
![Endpoints](./Service_Image.api/Docs/endpoints.png)

### Capture d'écran de l'inscription (Register)
![Register](./Service_Image.api/Docs/register.png)

### Capture d'écran de la connexion (Login)
![Login](./Service_Image.api/Docs/login.png)

### Capture d'écran de la modification d'image
![Modification d'image](./Service_Image.api/Docs/modif.png)

### Capture d'écran du téléversement d'image (Upload Image)
![Upload d'image](./Service_Image.api/Docs/upload%20image.png)

### Capture d'écran de la liste paginée des images
![Liste paginée des images](./Service_Image.api/Docs/get%20image.png)

---
 ## 🚀 Lancer le projet

1. Cloner le dépôt :

   ```bash
   git clone https://github.com/kelly-alphador/Service_Image.api.git
   cd Service_Image.api
2. Restaurer les dépendances :
   ```bash
   dotnet restore
3. Appliquer les migrations et créer la base de données : 
   ```bash
   dotnet ef database update -p Service_Image.api.Infrastructure.Core -s Service_Image.api
4. Démarrer l'application :
   ```bash
   dotnet run
