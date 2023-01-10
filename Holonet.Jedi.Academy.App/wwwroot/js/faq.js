var quotes = new Array();

$(document).ready(function () {
    PopulateQuotes();
    GetRandomQuote();
});

function PopulateQuotes() {
    quotes.push({ "quote": "I am a Jedi. I’m one with the Force, and the Force will guide me.", "author": "Ganodi" });
    quotes.push({ "quote": "For my ally is the Force, and a powerful ally it is.", "author": "Yoda" });
    quotes.push({ "quote": "Close your eyes. Feel it. The light…it’s always been there. It will guide you.", "author": "Maz Kanata" });
    quotes.push({ "quote": "Now I know there is something strong than fear — far stronger. The Force.", "author": "Kanan Jarrus" });
    quotes.push({ "quote": "Now, be brave and don't look back. Don't look back.", "author": "Shmi Skywalker" });
    quotes.push({ "quote": "You can kill me, but you will never destroy me. It takes strength to resist the dark side. Only the weak embrace it.", "author": "Obi-Wan Kenobi" });
    quotes.push({ "quote": "To answer power with power, the Jedi way this is not. In this war, a danger there is, of losing who we are", "author": "Yoda" });
    quotes.push({ "quote": "Once you start down the dark path, forever will it dominate your destiny. Consume you, it will.", "author": "Yoda" });
}

function GetRandomQuote() {
    var randomQuote = quotes[Math.floor(Math.random() * quotes.length)];
    $("#quote").html(randomQuote.quote);
    $("#quoteAuthor").html(randomQuote.author);
}