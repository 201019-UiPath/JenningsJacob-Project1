using GGsDB.Models;
using System.Collections.Generic;

namespace GGsDB.Repos
{
    public interface IVideoGameRepo
    {
        void AddVideoGame(VideoGame videoGame);
        void UpdateVideoGame(VideoGame videoGame);
        VideoGame GetVideoGame(int id);
        List<VideoGame> GetAllVideoGames();
        List<VideoGame> GetAllVideoGamesById(int id);
        void DeleteVideoGame(VideoGame videoGame);
    }
}