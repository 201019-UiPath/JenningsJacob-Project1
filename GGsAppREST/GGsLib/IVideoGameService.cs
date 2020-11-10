using GGsDB.Models;
using System.Collections.Generic;

namespace GGsLib
{
    public interface IVideoGameService
    {
        void AddVideoGame(VideoGame videoGame);
        void DeleteVideoGame(VideoGame videoGame);
        List<VideoGame> GetAllVideoGameById(int id);
        List<VideoGame> GetAllVideoGames();
        VideoGame GetVideoGame(int id);
        void UpdateVideoGame(VideoGame videoGame);
    }
}