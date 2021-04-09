using Microsoft.EntityFrameworkCore;
using System.Linq;
using SocialMedia.Data;
using SocialMedia.ViewModels;
using System;
using GeoCoordinatePortable;

namespace SocialMedia.Services
{
    public class BoardService
    {
        private readonly SocialMediaDbContext _dbContext;
        public int distance = 2;

        public BoardService(SocialMediaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public BoardList ListBoard()
        {
            var model = new BoardList();

            foreach (var board in _dbContext.Boards)
            {
                model.Boards.Add(new BoardList.Board
                {
                    Id = board.Id,
                    Name = string.IsNullOrEmpty(board.Name) ? string.Empty : board.Name
                });
            }

            return model;
        }

        public BoardList ListBoard(double latitude, double langitude)
        {
            var model = new BoardList();

            foreach (var board in _dbContext.Boards)
            {
                var sCoord = new GeoCoordinate(latitude, langitude);
                var eCoord = new GeoCoordinate(Convert.ToDouble(board.Latitude), Convert.ToDouble(board.Longitude));
                double result = sCoord.GetDistanceTo(eCoord) / 1000;
                if (result < distance)
                {
                    model.Boards.Add(new BoardList.Board
                    {
                        Id = board.Id,
                        Title = string.IsNullOrEmpty(board.Title) ? string.Empty : board.Title,
                        Name = string.IsNullOrEmpty(board.Name) ? string.Empty : board.Name,
                        Latitude = board.Latitude == null ? 0 : board.Latitude,
                        Longitude = board.Longitude == null ? 0 : board.Longitude
                    });
                }
            }

            return model;
        }

        public BoardView GetBoard(int id)
        {
            var model = new BoardView();

            var board = _dbContext.Boards
                .Include(b => b.Columns)
                .ThenInclude(c => c.Cards)
                .SingleOrDefault(x => x.Id == id);

            if (board == null)
                return model;
            model.Id = board.Id;
            model.Title = board.Title;

            foreach (var column in board.Columns)
            {
                var modelColumn = new BoardView.Column
                {
                    Title = column.Title,
                    Id = column.Id
                };

                foreach (var card in column.Cards)
                {
                    var modelCard = new BoardView.Card
                    {
                        Id = card.Id,
                        Content = card.Contents
                    };

                    modelColumn.Cards.Add(modelCard);
                }

                model.Columns.Add(modelColumn);
            }

            return model;
        }

        public void AddCard(AddCard viewModel)
        {
            var board = _dbContext.Boards
                .Include(b => b.Columns)
                .SingleOrDefault(x => x.Id == viewModel.Id);

            if (board != null)
            {
                var firstColumn = board.Columns.FirstOrDefault();
                var secondColumn = board.Columns.FirstOrDefault();
                var thirdColumn = board.Columns.FirstOrDefault();

                if (firstColumn == null || secondColumn == null || thirdColumn == null)
                {
                    firstColumn = new Models.Column { Title = "Todo" };
                    secondColumn = new Models.Column { Title = "Doing" };
                    thirdColumn = new Models.Column { Title = "Done" };
                    board.Columns.Add(firstColumn);
                    board.Columns.Add(secondColumn);
                    board.Columns.Add(thirdColumn);
                }

                firstColumn.Cards.Add(new Models.Card
                {
                    Contents = viewModel.Contents
                });
            }

            _dbContext.SaveChanges();
        }

        public void AddBoard(NewBoard viewModel)
        {
            _dbContext.Boards.Add(new Models.Board
            {
                Title = viewModel.Title,
                Latitude = viewModel.Latitude,
                Longitude = viewModel.Longitude,
                Name = viewModel.Name
            });

            _dbContext.SaveChanges();
        }

        public void Move(MoveCardCommand command)
        {
            var card = _dbContext.Cards.SingleOrDefault(x => x.Id == command.CardId);
            card.ColumnId = command.ColumnId;
            _dbContext.SaveChanges();
        }
    }
}
