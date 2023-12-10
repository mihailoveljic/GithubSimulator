using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces.Repositories;
using GitHubSimulator.Core.Interfaces.Services;
using GitHubSimulator.Core.Models.Entities;

namespace GitHubSimulator.Core.Services
{
    public class LabelService : ILabelService
    {
        private readonly ILabelRepository labelRepository;

        public LabelService(ILabelRepository labelRepository)
        {
            this.labelRepository = labelRepository;
        }

        public Task<bool> Delete(Guid id) =>
            labelRepository.Delete(id);

        public Task<Maybe<Label>> GetById(Guid id) =>
        labelRepository.GetById(id);

        public Task<IEnumerable<Label>> GetAll() =>
            labelRepository.GetAll();

        public Task<Label> Insert(Label label) =>
            labelRepository.Insert(label);

        public Task<Maybe<Label>> Update(Label label) =>
            labelRepository.Update(label);
    }
}
