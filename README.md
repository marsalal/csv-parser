# csv-parser

## Introduction
This is a sample webApi built using .net 7 and using minimal api design. The endpoint has been defined in the program.cs and uses a parser service that is injected per request. This endpoint does not require authentication and only accepts a file with `.csv` extension
The endpoint will parse the file contents into the following structure in a single string:

```
[Patient Name] [SSN] [Age] [Phone Number] [Status]
[Prescott, Zeke] [542-51-6641] [21] [801-555-2134] [Opratory=2,PCP=1]
[Goldstein, Bucky] [635-45-1254] [42] [435-555-1541] [Opratory=1,PCP=1]
[Vox, Bono] [414-45-1475] [51] [801-555-2100] [Opratory=3,PCP=2]
```

## Run
Simply run the solution and using postman can execute the `POST` endpoint that needs a file using a `multipart/form-data` with a key value of `formFile`

## Tests
This sample contains unit and integration tests. Those tests are written using XUnit and taking advantage of `WebApplicationFactory<T>` class provided by Microsoft.
Also a example file has been placed under the test project within the `resources` folder. 