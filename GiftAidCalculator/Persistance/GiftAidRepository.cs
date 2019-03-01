using System;
using System.Threading.Tasks;
using GiftAidCalculator.Models;
using Serilog;

namespace GiftAidCalculator.Persistance
{
    public interface IGiftAidRepository
    {
        Task CreateGiftAidDeclaration(GiftAidDeclarationRequest declaration);
    }

    public class GiftAidRepository : IGiftAidRepository
    {
        public async Task CreateGiftAidDeclaration(GiftAidDeclarationRequest declaration)
        {
            try
            {
                //awaited mock db insert
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error creating gift aid declaration: {@declaration}", declaration);
                throw ex;
            }
        }
    }
}
