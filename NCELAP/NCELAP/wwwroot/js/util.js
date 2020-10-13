//const baseUrl = 'https://localhost:44310/api/';
//const webBaseUrl = 'https://localhost:44374/';
const baseUrl = 'https://ncleas.herokuapp.com/api/';
const webBaseUrl = 'https://ncleas.herokuapp.com/';
//const webBaseUrl = 'https://ncelas.dpr.gov.ng/';
 

function extractExtensionFromFileName(fileName) {
    const lastDot = fileName.lastIndexOf('.');
    var extension = fileName.substring(lastDot + 1);

    return extension;
}