using AutoMapper;
using SkyRoute.Domains.Entities;
using SkyRoute.Domains.Models;
using SkyRoute.Extensions;
using SkyRoute.Helpers;
using SkyRoute.Services.Interfaces;
using SkyRoute.ViewModels;

namespace SkyRoute.Services
{
    public class FlightSearchHandler
    (
        IFlightSearchService _flightSearchService,
        IShoppingcartService _shoppingcartService,
        IMapper _mapper
    ) : IFlightSearchHandler
    {

        public async Task<FlightSearchResultVM> HandleFlightSearchAsync(HttpContext context, int? fromCityId, string? fromCity, int? toCityId, string? toCity, DateTime? departureDate, DateTime? returnDate, bool? isRetour, bool? isBusiness, int? adultPassengers, int? kidsPassengers)
        {
            var sessionCart = _shoppingcartService.GetShoppingCart(context.Session);
            var sessionSearch = sessionCart.FlightSearchSessionVM;
            var segmentSessionOutbound = sessionCart.OutboundFlights?.SegmentId;
            var segmentSessionRetour = sessionCart.RetourFlights?.SegmentId;

            // Als parameters ontbreken, val terug op session
            if (!fromCityId.HasValue || !toCityId.HasValue || !departureDate.HasValue || !adultPassengers.HasValue)
            {
                if (sessionSearch == null)
                {
                    throw new ArgumentException("Er zijn geen zoekcriteria opgegeven of opgeslagen in de sessie.");
                }

                fromCityId = sessionSearch.FromCityId;
                fromCity = sessionSearch.FromCity;
                toCityId = sessionSearch.ToCityId;
                toCity = sessionSearch.ToCity;
                departureDate = sessionSearch.OutboundFlightDate;
                returnDate = sessionSearch.ReturnFlightDate;
                isRetour = sessionSearch.TripType == TripType.Retour;
                isBusiness = sessionSearch.TripClass == TripClass.Business;
                adultPassengers = sessionSearch.AdultPassengers;
                kidsPassengers = sessionSearch.KidsPassengers;
            }



            // vlucht ophalen

            FlightSearchResult result = await _flightSearchService.SearchFlightsAsync(
                fromCityId.Value,
                toCityId.Value,
                departureDate.Value,
                returnDate,
                isBusiness ?? false,
                isRetour ?? false,
                adultPassengers.Value,
                kidsPassengers
            );

            var viewModel = _mapper.Map<FlightSearchResultVM>(result);

            viewModel.SelectedOutboundSegment = segmentSessionOutbound;
            viewModel.SelectedRetourSegment = segmentSessionRetour;

            viewModel.FormModel = new FlightSearchFormVM
            {
                DepartureCity = fromCityId.Value,
                DestinationCity = toCityId.Value,
                DepartureDate = departureDate.Value,
                ReturnDate = returnDate?.Date,
                SelectedTripClass = isBusiness.GetValueOrDefault() ? TripClass.Business : TripClass.Economy,
                SelectedTripType = isRetour.GetValueOrDefault() ? TripType.Retour : TripType.Enkel,
                AdultPassengers = adultPassengers.Value,
                KidsPassengers = kidsPassengers,
            };

            // Session updaten

            sessionCart.FlightSearchSessionVM = new FlightSearchSessionVM
            {
                FromCityId = fromCityId.Value,
                FromCity = fromCity ?? "Onbekend",
                ToCityId = toCityId.Value,
                ToCity = toCity ?? "Onbekend",
                OutboundFlightDate = departureDate.Value,
                ReturnFlightDate = returnDate?.Date,
                AdultPassengers = adultPassengers.Value,
                KidsPassengers = kidsPassengers,
                TripClass = isBusiness.GetValueOrDefault() ? TripClass.Business : TripClass.Economy,
                TripType = isRetour.GetValueOrDefault() ? TripType.Retour : TripType.Enkel
            };

            _shoppingcartService.SetShoppingObject(sessionCart, context.Session);

            return viewModel;
        }


        public async Task<object> GetSelectedFlightSegmentAsync(FlightSelectionVM selection, ISession session)
        {
            if (selection.SegmentId == Guid.Empty)
                throw new ArgumentException("Ongeldige segmentId");

            FlightSegmentGroup flightSegmentGroup = await _flightSearchService.GetAvailableFlights(
                selection.SegmentId,
                selection.IsBusiness,
                selection.AdultPassengers,
                selection.KidsPassengers);

            if (flightSegmentGroup.Flights.Count == 0)
            {
                return new { success = false, selectedSegment = selection.SegmentId };
            }

            var shoppingCartVM = _shoppingcartService.GetShoppingCart(session);

            var existingItem = selection.IsRetour
                ? shoppingCartVM.RetourFlights?.SegmentId == selection.SegmentId
                : shoppingCartVM.OutboundFlights?.SegmentId == selection.SegmentId;

            if (!existingItem)
            {
                var segmentSession = new FlightSegmentSessionVM
                {
                    SegmentId = selection.SegmentId,
                    Flights = [.. flightSegmentGroup.Flights.Select(f => f.Id)],
                    TotalDuration = flightSegmentGroup.TotalDuration,
                    TotalPrice = selection.IsBusiness
                        ? flightSegmentGroup.Flights.Sum(f => f.PriceBusiness)
                        : flightSegmentGroup.Flights.Sum(f => f.PriceEconomy)
                };

                if (!selection.IsRetour)
                {
                    shoppingCartVM.OutboundFlights = segmentSession;
                }
                else
                {
                    shoppingCartVM.RetourFlights = segmentSession;
                }

                _shoppingcartService.SetShoppingObject(shoppingCartVM, session);
            }

            return new { success = true, selectedSegment = selection.SegmentId };
        }


    }
}