// const baseUrl = 'https://localhost:44310/api/';
//const baseUrl = 'https://ncelap-demo-api.azurewebsites.net/api/';
const baseUrl = 'https://ncelaapi.azurewebsites.net/api/';


function extractExtensionFromFileName(fileName) {
    const lastDot = fileName.lastIndexOf('.');
    var extension = fileName.substring(lastDot + 1);

    return extension;
}