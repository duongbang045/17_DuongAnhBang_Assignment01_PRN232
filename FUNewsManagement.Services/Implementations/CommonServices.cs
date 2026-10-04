using FUNewsManagement.Models;
using FUNewsManagement.Models.DTOs;
using FUNewsManagement.Repositories.Interfaces;
using FUNewsManagement.Services.Interfaces;

namespace FUNewsManagement.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly ISystemAccountRepository _accountRepository;

        public AccountService(ISystemAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public IEnumerable<SystemAccount> GetAll() => _accountRepository.GetAll();

        public SystemAccount GetById(short id) => _accountRepository.GetById(id);

        public IEnumerable<SystemAccount> Search(string keyword) => _accountRepository.SearchByNameOrEmail(keyword);

        public SystemAccount GetByEmail(string email) => _accountRepository.GetByEmail(email);

        public void Insert(SystemAccount account) => _accountRepository.Insert(account);

        public void Update(SystemAccount account) => _accountRepository.Update(account);

        public (bool success, string message) Delete(short id)
        {
            if (_accountRepository.HasCreatedNews(id))
            {
                return (false, "Cannot delete account: this account has created news articles.");
            }
            var account = _accountRepository.GetById(id);
            if (account == null)
            {
                return (false, "Account not found.");
            }
            _accountRepository.Delete(account);
            return (true, "Account deleted successfully.");
        }
    }

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IEnumerable<Category> GetAll() => _categoryRepository.GetAll();

        public Category GetById(short id) => _categoryRepository.GetById(id);

        public IEnumerable<Category> Search(string keyword) => _categoryRepository.SearchByName(keyword);

        public void Insert(Category category) => _categoryRepository.Insert(category);

        public void Update(Category category) => _categoryRepository.Update(category);

        public (bool success, string message) Delete(short id)
        {
            if (_categoryRepository.HasNewsArticles(id))
            {
                return (false, "Cannot delete category: this category is already used in news articles.");
            }
            var category = _categoryRepository.GetById(id);
            if (category == null)
            {
                return (false, "Category not found.");
            }
            _categoryRepository.Delete(category);
            return (true, "Category deleted successfully.");
        }
    }

    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;

        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public IEnumerable<Tag> GetAll() => _tagRepository.GetAll();

        public Tag GetById(int id) => _tagRepository.GetById(id);

        public IEnumerable<Tag> Search(string keyword) => _tagRepository.SearchByName(keyword);

        public void Insert(Tag tag) => _tagRepository.Insert(tag);

        public void Update(Tag tag) => _tagRepository.Update(tag);

        public void Delete(int id)
        {
            var tag = _tagRepository.GetById(id);
            if (tag != null) _tagRepository.Delete(tag);
        }
    }

    public class ProfileService : IProfileService
    {
        private readonly ISystemAccountRepository _accountRepository;

        public ProfileService(ISystemAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public SystemAccount GetProfile(short accountId) => _accountRepository.GetById(accountId);

        public void UpdateProfile(short accountId, UpdateProfileDto dto)
        {
            var account = _accountRepository.GetById(accountId);
            if (account == null) return;
            if (!string.IsNullOrEmpty(dto.AccountName)) account.AccountName = dto.AccountName;
            if (!string.IsNullOrEmpty(dto.AccountEmail)) account.AccountEmail = dto.AccountEmail;
            if (!string.IsNullOrEmpty(dto.AccountPassword)) account.AccountPassword = dto.AccountPassword;
            _accountRepository.Update(account);
        }
    }
}
