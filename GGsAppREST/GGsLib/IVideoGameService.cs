using GGsDB.Models;
using System.Collections.Generic;

namespace GGsLib
{
    public interface IVideoGameService
    {
        void AddVideoGame(VideoGame videoGame);
        void DeleteVideoGame(VideoGame videoGame);
        List<VideoGame> GetAllVideoGames();
        VideoGame GetVideoGameById(int id);
        void UpdateVideoGame(VideoGame videoGame);
    }
}