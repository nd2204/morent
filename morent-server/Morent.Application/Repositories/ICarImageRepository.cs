using System;

namespace Morent.Application.Repositories;

public interface ICarImageRepository
{
  Task<IEnumerable<MorentCarImage>> GetByCarIdAsync(Guid carId);
  Task<MorentCarImage> GetByIdAsync(Guid id);
  Task<MorentCarImage> AddAsync(MorentCarImage carImage);
  Task<bool> DeleteAsync(Guid id);
  Task UpdateAsync(MorentCarImage carImage);
}
