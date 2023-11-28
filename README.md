# RutaAragonEmprende2023
Juego WebGL para móviles desarrollado para La Semana del Emprendimiento en Aragón 2023
El juego consta de 2 minijuegos diferentes:
Por un lado, un quizz de respuesta múltiple estructurado en 4 categorías.
Por otro, 4 escenarios aragoneses que hay que recorrer, esquivando los obstáculos, en forma de scroll vertical.

El código de este proyecto permite replicar esta experiencia de manera total o parcial, mientras que tanto el arte como el contenido narrativo y las preguntas no pueden ser reutilizadas para otros proyectos.

CONFIGURACIÓN
El proyecto está diseñado para ser alojado en un servidor, y funcionar de manera autónoma. Tanto las preguntas y respuestas, como la narrativa dependen de dos archivos independientes, lo que permite sustituirlos para generar una nueva experiencia. Los archivos se incluyen por defecto en la build, así que con modificarlos antes de crear la build es suficiente.
El proyecto cuenta además con una integración de Firebase Realtime Database, que permite trackear el desempeño de la aplicación, acumulando los datos necesarios para obtener las métricas de uso y engagement. Será necesario crear y conectar con la base de datos apropiada.
En su versión original, el proyecto finalizaba con un lin a un formulario de Google que servía para la recogida de datos. En caso de querer usar esta opción, deberá crearse un formulario para ello.
