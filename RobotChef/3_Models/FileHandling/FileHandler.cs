using Newtonsoft.Json;
using RobotChefProject._2_Domain;
using System.Reflection;
using System.Runtime.Intrinsics.Arm;

namespace RobotChefProject._3_Models.FileHandling
{
    public class FileHandler
    {
        #region Variables
        private static string _baseFolderPath => Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        private readonly string _resourceFolderPath = _baseFolderPath + @"\1_Resources";
        #endregion

        #region Constructor
        public FileHandler()
        {
            EnsureDirectoriesExist();
        }
        public FileHandler(string gameId) : base()
        {
            EnsureDirectoriesExist();
        }
        #endregion

        #region Public Methods

        #region Get FilePaths
        /// <summary>
        /// Gets the image path for a given image using name
        /// </summary>
        /// <param name="imageName">Name of the image file</param>
        /// <returns>The image path to the given image</returns>
        public string? GetImagePath(string imageName)
        {
            string? imagePath = null;
            if (!imageName.Equals(""))
            {
                imagePath = Path.Combine(_resourceFolderPath, $"/{imageName}.png");
            }
            return imagePath;
        }
        public Recipe? GetRecipe(string recipeName)
        {
            if (!recipeName.Equals(""))
            {
                string recipeJson = File.ReadAllText($"{_resourceFolderPath}/{recipeName}.json");
                return JsonConvert.DeserializeObject<Recipe>(recipeJson);
            }
            else
            {
                throw new FileNotFoundException($"Recipe file not found for {recipeName}");
            }
        }
        #endregion

        #endregion

        #region Private Method - Ensure Directories Exist
        /// <summary>
        /// Ensure that default directories exits
        /// </summary>
        private void EnsureDirectoriesExist()
        {
            if (!Directory.Exists(_baseFolderPath))
            {
                throw new DirectoryNotFoundException($"baseFolderPath folder not found: {_baseFolderPath}");
            }
            if (!Directory.Exists(_resourceFolderPath))
            {
                throw new DirectoryNotFoundException($"resourceFolderPath folder not found: {_resourceFolderPath}");
            }
        }
        #endregion
    }
}
