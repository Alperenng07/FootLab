using AutoMapper;
using FootLab.Entities.DTOs;
using FootLab.Entities.Entites;
namespace FootLab.Bussines.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {

            //CreateMap<User, UserRegisterDto>().ReverseMap();

            // --- LEAGUE MAPPINGS ---
            CreateMap<LeagueForPostDto, League>();
            CreateMap<LeagueForUpdateDto, League>();
            CreateMap<League, LeagueResultDto>();

            // --- GROUP MAPPINGS ---
            CreateMap<GroupForPostDto, Group>();
            CreateMap<GroupForUpdateDto, Group>();
            CreateMap<Group, GroupResultDto>()
                .ForMember(dest => dest.LeagueName, opt => opt.MapFrom(src => src.League.Name));

            // --- TEAM MAPPINGS ---
            CreateMap<TeamForPostDto, Team>();
            CreateMap<TeamForUpdateDto, Team>();
            CreateMap<Team, TeamResultDto>();

            // --- PLAYER MAPPINGS ---
            CreateMap<PlayerForPostDto, Player>();
            CreateMap<PlayerForUpdateDto, Player>();
            CreateMap<Player, PlayerResultDto>()
                .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Position.ToString()))
                .ForMember(dest => dest.TeamName, opt => opt.MapFrom(src => src.Team != null ? src.Team.Name : "Serbest"))
                .ForMember(dest => dest.PositionId, opt => opt.MapFrom(src => (int?)src.Position));

            // --- MATCH MAPPINGS ---
            CreateMap<MatchForPostDto, Match>();
            CreateMap<MatchForUpdateDto, Match>();
            CreateMap<Match, MatchResultDto>()
                .ForMember(dest => dest.HomeTeamName, opt => opt.MapFrom(src => src.HomeTeam.Name))
                .ForMember(dest => dest.AwayTeamName, opt => opt.MapFrom(src => src.AwayTeam.Name))
                .ForMember(dest => dest.HomeTeamLogo, opt => opt.MapFrom(src => src.HomeTeam.LogoUrl))
                .ForMember(dest => dest.AwayTeamLogo, opt => opt.MapFrom(src => src.AwayTeam.LogoUrl));

            // --- TEAM SEASON DETAIL MAPPINGS ---
            CreateMap<TeamSeasonDetailForPostDto, TeamSeasonDetail>();
            CreateMap<TeamSeasonDetail, TeamSeasonDetailResultDto>()
                .ForMember(dest => dest.TeamName, opt => opt.MapFrom(src => src.Team.Name))
                .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.Group.Name));

            // --- STANDING MAPPINGS ---
            CreateMap<StandingForPostDto, Standing>();
            CreateMap<Standing, StandingResultDto>()
                .ForMember(dest => dest.TeamName, opt => opt.MapFrom(src => src.Team.Name));

            // --- GOAL MAPPINGS ---
            CreateMap<GoalForPostDto, Goal>();
            CreateMap<Goal, GoalResultDto>()
                .ForMember(dest => dest.PlayerFullName, opt => opt.MapFrom(src => $"{src.Player.FirstName} {src.Player.LastName}"))
                .ForMember(dest => dest.TeamName, opt => opt.MapFrom(src => src.Team.Name));
        }
    }
    }

