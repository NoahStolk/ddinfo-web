$(document).ready(function () {
	var order = [23, 27, 25, 24];
	var index = 0;

	$(document).on("click", ".secret-tile", function () {
		$(this).removeClass("secret-tile");
		var height = parseInt($(this).attr("tile-height"));
		var color = "rgb(" + height + ", " + height / 2 + ", 0)";
		$(this).css({ backgroundColor: color });

		var id = parseInt($(this).attr("id"));
		if (order[index] !== id) {
			$(".secret-tile").each(function (index, element) {
				$(element).removeClass("secret-tile");
				var height = parseInt($(element).attr("tile-height"));
				var color = "rgb(" + height + ", " + height / 2 + ", 0)";
				$(element).css({ backgroundColor: color });

				var audioElement = document.createElement("audio");
				audioElement.setAttribute("src", "audio/daggerhit1.mp3");
				audioElement.play();
			});
		}
		else {
			index++;

			var audioElement = document.createElement("audio");
			audioElement.setAttribute("src", "audio/ricochet" + index + ".mp3");
			audioElement.play();
		}

		if (index === 4) {
			alert("you won!!!!!");
		}
	});
});