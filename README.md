# ALDABACountryApi

Esta es una aplicación API sencilla para solicitar datos detallados sobre países, actuando como intermediario de la web restcountries.com.
La API contiene una caché con permanencia de 2 minutos para evitar llamadas repetidas a restcountries, así como documentación de Swagger (disponible ejecutando el proyecto en un entorno de desarrollo),
un proyecto de prueba con tests unitarios y de integración, y la capacidad de ser ejecutado en contenedores de Docker.

# Dependencias

El proyecto se creó utilizando Visual Studio 2022, es recomendable usarlo para abrir el proyecto. Se requiere tener Docker instalado para poder ejecutar el programa en un contenedor de Windows.

# Pasos de instalación

La aplicación puede ejecutarse desde Visual Studio, como una aplicación independiente tras ser publicada, o como una imagen de Docker.

Para utilizar la aplicación en un contenedor de Docker se requieren los siguientes pasos:
1. Descargar el repositorio
2. Generar una imagen de docker de la solución desde la carpeta del proyecto web (eg: "docker build -f Dockerfile ..")
3. Ejecutar un contenedor con la nueva imagen

Ejecutar el proyecto desde Visual Studio hace que las peticiones sean escuchadas por la url https://localhost:7098, en caso contrario se utilizará la url por defecto http://localhost:5000

# Ejemplos de uso
La aplicación contiene dos endpoints:
1. country/{nameOrCode} - Devuelve información detallada del país encontrado usando nameOrCode como nombre (en inglés) o como código ISO.
Ejemplo: https://localhost:7098/country/ES devolverá información detallada de España
2. country/{nameOrCode}/neighbors - Devuelve una lista de los países vecinos del encontrado usando nameOrCode como nombre (en inglés) o como código ISO.
Ejemplo: https://localhost:7098/country/ES/neighbors devolverá una lista de los países vecinos a España
