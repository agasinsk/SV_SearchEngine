# SV Search Engine
Web-based search engine prototype as part of coding case for SV. 

### Description
The engine supports custom weight configurations (think of 'weight' as of importance to user) in which the object may have different weights assigned for different properties. What's more, the object may also have transitive weight put on properties in case it's assigned to other object (e.g. a Lock belonging to a Building may also have certain weights set for Building properties). Apart from that, the search engine differentiates full search from partial search so that full-match results are 10 times more relevant.

Implemented using ASP.NET Core 3.1 and Angular 8. 

### Live example

Working live example is available at: https://ag-sv-search-engine.herokuapp.com/

As there's currently no direct support for .NET on Heroku, it was deployed as a Docker image.

## How to run

### Visual Studio

The standard way is to run using Visual Studio by simply opening the solution file, building and running it. Note that it requires .NET Core Runtime and node.js installed on your system.

### Docker support
Attached Docker file enables you to run the app in a Docker Container. Build and run the Docker image using (names and ports are examples only):

<code>docker build . -t ag-sv-search-engine</code>

<code>docker run -d -p 8090:80 ag-sv-search-engine </code>
