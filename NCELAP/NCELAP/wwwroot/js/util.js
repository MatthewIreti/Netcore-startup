const baseUrl = 'https://localhost:44310/api/';
const webBaseUrl = 'https://localhost:44374/';
//const baseUrl = 'https://ncleasapi.herokuapp.com/api/';
//const webBaseUrl = 'https://ncleas.herokuapp.com/';
//const webBaseUrl = 'https://ncelas.dpr.gov.ng/';


//Upload names
 const  DeclarationSignatureFileName = "_declaration_signature";
 const  HasLicenseRefusedFileName = "_licenserefused";
 const  HasLicenseRevokedFileName = "_licenserevoked";
 const  HasRelatedLicenseFileName = "_relatedlicense";
 const  HoldRelatedLicenseFileName = "_holdrelatedlicense";
 const  ProposedArrangementAttachmentFileName = "_proposedarrangementlicense";
 const  OPLFileName = "_OPL_License";
 const  SafetyCaseFileName = "_SafetyCaseApproved";
 const  SCADAFileName = "_SCADA_System";
 const  GTSFileName = "_Gas_transmission_system";
 const  TechnicalAttributeFileName = "_technical_attributes";
 const  AuxiliarySystemFileName = "_Auxiliary_systems";
 const  TariffAndPricingFileName = "_Tarrif_and_pricing";
 const  RiskManagementFileName = "_Risk_management";
 const  CommunityMOUFileName = "_CommunityMOU";

function extractExtensionFromFileName(fileName) {
    const lastDot = fileName.lastIndexOf('.');
    var extension = fileName.substring(lastDot + 1);

    return extension;
}