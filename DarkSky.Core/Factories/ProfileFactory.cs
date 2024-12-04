using DarkSky.Core.Services;
using DarkSky.Core.ViewModels.Temporary;
using FishyFlip.Lexicon.App.Bsky.Actor;
using FishyFlip.Lexicon.App.Bsky.Feed;
using FishyFlip.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DarkSky.Core.Factories
{
	/*
	 * Factory method to return ProfileViewModel from ProfileViewBasic, Handles and DID
	 */
	public class ProfileFactory
	{
		public static async Task<ProfileViewModel> Create(ProfileViewBasic profileView)
			=> await ProfileFactory.Create(profileView.Did);

		public static async Task<ProfileViewModel> Create(ATIdentifier Identifier)
		{
			var proto = ServiceContainer.Services.GetService<ATProtoService>().ATProtocolClient;
			var profile = await proto.Actor.GetProfileAsync(Identifier);
			return new ProfileViewModel(profile.AsT0);
		}
	}
}
