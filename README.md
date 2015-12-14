# Spotify Metadata Client

## Descripción

Esta aplicación es un consumidor de datos de Spotify, usando el servicio provisto por ellos llamado Metadata API. Se basa en una aplicación ASP.NET MVC (lenguaje C#), con integración Entity Framework (enfoque Code First) con la base de datos, consultas en LINQ, y persistencia provista por SQL Server Express LocalDB. 

## Modo de uso

Una vez en el menú de la aplicación, ingrese el nombre de un artista en la barra de búsqueda. Esto automáticamente guardará toda la información que tiene Spotify relacionada con este artista en su base de datos. Luego, haga click en artistas, en la barra de navegación superior, y seleccione alguno de los artistas descargados para ver información relevante a sus álbumes.

## Funcionamiento

La aplicación cuenta con una página de inicio que permite al usuario buscar por texto algún artista en Spotify. Una vez realizada la consulta, se hace un _request_ al servicio y se procesan sus datos para formar un objeto de tipo JSON. Luego, se guardan todos los artistas, álbumes, y canciones encontrados en la base de datos, representados como entidades EF que almacenan los datos más significativos. El almacenamiento de los datos se hace automáticamente después de haber realizado la búsqueda, y se le entrega un mensaje al usuario confirmándoselo.

Haciendo click en la pestaña artistas el usuario puede encontrar todos los artistas descargados, y seleccionar alguno de ellos para ver información relevante acerca de sus álbumes, como popularidad promedio de sus canciones, canción más larga y duración de esta, todos ordenados de menos a más reciente año de lanzamiento.

## Requerimientos

Para poder ejecutar la aplicación correctamente se debe importar como proyecto a un ambiente Visual Studio 2012 o posterior, con SQL Server Express LocalDB instalado, y las librerías necesarias (en este caso es necesario contar con el paquete de Entity Framework).

## Consideraciones

Se está revisando un error con respecto a la duplicación de álbumes, que debería solucionarse en una versión posterior.