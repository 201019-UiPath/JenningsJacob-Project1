using GGsDB.Models;
using GGsDB.Repos;
using System.Collections.Generic;

namespace GGsLib
{
    public class VideoGameService
    {
        IVideoGameRepo repo;
        public VideoGameService(IVideoGameRepo repo)
        {
            this.repo = repo;
        }
        public void AddVideoGame(VideoGame videoGame)
        {
            repo.AddVideoGame(videoGame);
        }
        public void DeleteVideoGame(VideoGame videoGame)
        {
            repo.DeleteVideoGame(videoGame);
        }
        public List<VideoGame> GetAllVideoGames()
        {
            return repo.GetAllVideoGames();
        }
        public List<VideoGame> GetAllVideoGameById(int id)
        {
            return repo.GetAllVideoGamesById(id);
        }
        public VideoGame GetVideoGame(int id)
        {
            return repo.GetVideoGame(id);
        }
        public void UpdateVideoGame(VideoGame videoGame)
        {
            repo.UpdateVideoGame(videoGame);
        }
    }
}