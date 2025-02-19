async function fetchSuggestions() {
    let query = document.getElementById("search-box").value;
    if (query.length === 0) {
        document.getElementById("suggestions").innerHTML = "";
        return;
    }
    let response = await fetch(`/search?q=${query}`);
    let suggestions = await response.json();
    let suggestionBox = document.getElementById("suggestions");
    console.log(suggestions);
    suggestionBox.innerHTML = "";
    suggestions.forEach(word => {
        let li = document.createElement("li");
        let p = document.createElement("p");
        let p2 = document.createElement("p");
        p.innerText = word;
        p2.innerText ="X";
        p2.style.color = "red";
        p2.style.cursor = "pointer";
        p2.addEventListener("click", () => deleteWord(p.innerText
        ));
       

        let span = document.createElement("span");

        span.appendChild(p);
        span.appendChild(p2);
        span.style.display = "flex";
        span.style.justifyContent = "space-around";
        span.style.alignItems = "center";
        li.appendChild(span);
        suggestionBox.appendChild(li);
    });
}

async function insertWord() {
    let word = document.getElementById("search-box").value;
    if (!word) return;

    await fetch("/insert", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ word: word })
    });

    document.getElementById("search-box").value = "";
    fetchSuggestions();
}

async function deleteWord(word) {
    console.log(word);
    await fetch("/delete", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ word: word })
    });

    let items = document.querySelectorAll("#suggestions li");
    items.forEach(li => {
        if (li.innerText.includes(word)) {
            li.remove(); 
        }
    });
}

