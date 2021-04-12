export interface ImmunizationRecord {
  fullUrl: string;
  id: string;
  patient: {
    fullName: string;
    birthDate: string;
    identifier: {
      value: string;
      system: string;
    };
    reference: string;
  };
  vaccine: {
    identifier: {
      value: string;
      system: string;
    };
    manufacturer: string;
    productName: string;
    disease: string;
  };
  lotNumber: string;
  dateOfVaccination: string;
  doseNumber: number;
  countryOfVaccination: string;
  administeringCentre: string;
  nextVaccinationDate: string;
}
