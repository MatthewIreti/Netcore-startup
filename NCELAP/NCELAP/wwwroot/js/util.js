//const baseUrl = 'https://localhost:44310/api/';
//const webBaseUrl = 'https://localhost:44374/';
const baseUrl = 'https://ncelaapi.azurewebsites.net/api/';
const webBaseUrl = 'https://ncelas.dpr.gov.ng/';
//const baseUrl = 'https://ncelap-demo-api.azurewebsites.net/api/';
//const webBaseUrl = 'https://ncelap-demo.azurewebsites.net/';
 

function extractExtensionFromFileName(fileName) {
    const lastDot = fileName.lastIndexOf('.');
    var extension = fileName.substring(lastDot + 1);

    return extension;
}