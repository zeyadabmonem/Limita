using Limita.Business.Common;
using Limita.Business.DTOs.Beneficiary;
using Limita.Business.Services.Interface;
using Limita.Data.Entities;
using Limita.Data.Repo.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Limita.Business.Services.Implementation
{
    public class BeneficiaryService : IBeneficiaryService
    {
        private readonly IBeneficiaryRepo beneficiaryRepo;

        public BeneficiaryService(IBeneficiaryRepo beneficiaryRepo)
        {
            this.beneficiaryRepo = beneficiaryRepo;
        }
        public async Task<ServiceResult<BeneficiaryResponseDTO>> AddBeneficiary(int userId, AddBeneficiaryRequestDTO requestDTO)
        {
            if ( await beneficiaryRepo.ExistingByUseridAndIdentifierAsync(userId, requestDTO.AccountIdentifier)) 
                return new ServiceResult<BeneficiaryResponseDTO> { Success = false, Message = "Beneficiary already exists" };

            var beneficiary = new Beneficiary { AccountIdentifier = requestDTO.AccountIdentifier, 
                CreatedAt = DateTime.UtcNow,
                Name = requestDTO.Name,
                UserId = userId };

            int id = await beneficiaryRepo.AddBeneficiaryAsync(beneficiary);

            var data = new BeneficiaryResponseDTO { AccountIdentifier = requestDTO.AccountIdentifier, Id = id, Name = requestDTO.Name };

            return new ServiceResult<BeneficiaryResponseDTO> { Success = true, Message = "Beneficiary created successfully", Data = data };
        }

        public async Task<ServiceResult<bool>> DeleteBeneficiary(int userId, int beneficiaryId)
        {
            var beneficairy = await beneficiaryRepo.GetBeneficiaryAsync(beneficiaryId);

            if (beneficairy == null || beneficairy.UserId != userId)
                return new ServiceResult<bool> { Success = false, Message = "Invalid operation" };

            await beneficiaryRepo.DeleteBeneficiaryAsync(beneficiaryId);

            return new ServiceResult<bool> { Success = true, Message = "beneficairy deleted successfully" };
        }

        public async Task<ServiceResult<List<BeneficiaryResponseDTO>>> GetAllBeneficiaries(int userId)
        {
            var beneficiaries = await beneficiaryRepo.GetBeneficiariesAsync(userId);
            List<BeneficiaryResponseDTO> dtos = new();
            foreach (var b in beneficiaries)
            {
                dtos.Add(new BeneficiaryResponseDTO { Id = b.Id, AccountIdentifier = b.AccountIdentifier, Name = b.Name });
            }

            return  new ServiceResult<List<BeneficiaryResponseDTO>> { Success = true, Message = "Data restored successfully", Data = dtos };
        }

        public async Task<ServiceResult<BeneficiaryResponseDTO>> GetBeneficiaryById(int userId, int beneficiaryId)
        {
            var beneficairy = await beneficiaryRepo.GetBeneficiaryAsync(beneficiaryId);
            
            if(beneficairy == null || beneficairy.UserId != userId)
                return new ServiceResult<BeneficiaryResponseDTO> { Success = false, Message = "Invalid operation" };

           
            return new ServiceResult<BeneficiaryResponseDTO> { Success = true, Message = "beneficairy retrieved successfully" , Data =new BeneficiaryResponseDTO { Id= beneficairy.Id , AccountIdentifier = beneficairy.AccountIdentifier, Name = beneficairy .Name} };

        }

        public async Task<ServiceResult<BeneficiaryResponseDTO>> UpdateBeneficiary(int userId, int beneficiaryId, UpdateBeneficiaryRequestDTO requestDTO)
        {
            var beneficairy = await beneficiaryRepo.GetBeneficiaryAsync(beneficiaryId);



            if (beneficairy == null || beneficairy.UserId != userId)
                return new ServiceResult<BeneficiaryResponseDTO> { Success = false, Message = "Invalid operation" };

            beneficairy.AccountIdentifier = requestDTO.AccountIdentifier;
            beneficairy.Name = requestDTO.Name;

           await beneficiaryRepo.UpdateBeneficiaryAsync(beneficairy);

            return new ServiceResult<BeneficiaryResponseDTO> { Success = true, Message = "beneficairy updated successfully", Data = new BeneficiaryResponseDTO { Id = beneficairy.Id, AccountIdentifier = requestDTO.AccountIdentifier, Name = requestDTO.Name } };

        }
    }
}
