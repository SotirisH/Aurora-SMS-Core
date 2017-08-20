using Aurora.Insurance.Data;
using Aurora.Insurance.EFModel;
using System;
using System.Linq;

namespace Aurora.SMS.Web
{
    public class DbInsuranceSeed
    {
        public void Seed(InsuranceDb context)
        {
            // Speed the insert process by turning off the AutoDetectChangesEnabled
            context.ChangeTracker.AutoDetectChangesEnabled = false;
            var faker = new Bogus.Faker();
            // Mock companies
            if (!context.Companies.Any())
            {
                for (var i = 0; i < 10; i++)
                {
                    context.Companies.Add(new Company()
                    {
                        Id = faker.Random.UShort().ToString(),
                        Description = faker.Company.CompanyName()
                    }
                                );
                }
                context.SaveChanges();
            }
            // populate DB with mock entities
            if (!context.Contracts.Any())
            {
                for (var i = 0; i < 1000; i++)
                {
                    CreateMockContract(context);
                }
            }

        }

        private void CreateMockContract(InsuranceDb context)
        {


            var faker = new Bogus.Faker();
            var contractCompany = context.Companies.OrderBy(c => c.Id).Skip(faker.Random.UShort(0, (ushort)(context.Companies.Count() - 1))).First();
            var mockPerson = new Bogus.Person();

            var newPerson = new Person()
            {
                Address = mockPerson.Address.Street,
                BirthDate = mockPerson.DateOfBirth,
                DrivingLicenceNum = faker.Lorem.Letter().ToUpper() + faker.Lorem.Letter().ToUpper() + "-" + faker.Random.UInt(10000, 99999).ToString(),
                FatherName = faker.Name.FirstName(),
                FirstName = mockPerson.FirstName,
                LastName = mockPerson.LastName,
                TaxId = faker.Random.UInt(1000000, 9999999).ToString(),
                ZipCode = mockPerson.Address.ZipCode,
            };

            context.Persons.Add(newPerson);
            // Create 1-3 telephone numbers
            for (var phoneIdx = 0; phoneIdx < faker.Random.UShort(1, 3); phoneIdx++)
            {
                var mockPhone = new Phone();
                mockPhone.Person = newPerson;
                mockPhone.Number = faker.Phone.PhoneNumber();
                mockPhone.PhoneType = faker.PickRandom<PhoneType>();
                context.Phones.Add(mockPhone);
            }

            //Create 1-5 contracts for the user by setting a random start date for the first contract
            //In the insurance market the cotracts a usually 6 months length

            DateTime nextStartDate = DateTime.Today.AddMonths((-1) * faker.Random.UShort(1, 24));
            // pick a random company

            string contractNumber = faker.Random.Number(1000000, 9999999).ToString();
            do
            {
                // create a contract
                Contract contract = new Contract();
                contract.Person = newPerson;
                contract.Company = contractCompany;
                contract.ContractNumber = contractNumber;
                contract.IssueDate = nextStartDate.AddDays(-2);
                contract.ExpireDate = nextStartDate.AddMonths(6);
                contract.GrossAmount = faker.Random.Number(100, 1000);
                contract.TaxAmount = (decimal)((double)contract.GrossAmount * (10d / 100d));
                contract.NetAmount = contract.GrossAmount - contract.TaxAmount;
                contract.PlateNumber = faker.Lorem.Letter().ToUpper() +
                    faker.Lorem.Letter().ToUpper() +
                    faker.Lorem.Letter().ToUpper() +
                    "-" +
                    faker.Random.UInt(10000, 99999).ToString();
                contract.ReceiptNumber = faker.Random.Number(10000000, 99999999).ToString();
                contract.StartDate = nextStartDate;
                context.Contracts.Add(contract);

                nextStartDate = nextStartDate.AddMonths(6);

            } while (nextStartDate < DateTime.Today);
            context.SaveChanges();
        }
    }
}
