📋 RECOMENDACIONES TÉCNICAS

🔐 Seguridad

JWT Expiración : Limitar vida útil del token (ej: 1 hora).
Renovación de tokens : Agregar refresh token si es necesario.

CSRF Protection : Usar tokens CSRF si se usan formularios dinámicos.
Input Validation : Validar siempre en backend los campos recibidos.
No guardar tokens en logs : Evitar registrar JWT o contraseñas.

⚙️ Escalabilidad

Para soportar más de 100 usuarios concurrentes:

Usar balanceador de carga (Nginx, HAProxy)
Implementar caché (Redis) para sesiones
Usar contenedores (Docker) y orquestación (Kubernetes)

🧪 Pruebas

Agregar pruebas unitarias con xUnit o NUnit.
Probar cada endpoint desde Swagger UI.
Simular concurrencia con herramientas como Postman Runner o JMeter.

📦 Deploy

Publicar backend en IIS, Kestrel, o Azure App Service.
Servir frontend desde servidor web ligero (Nginx, Apache).
Usar HTTPS en producción.
Automatizar deploy con GitHub Actions, Jenkins o Azure DevOps.
