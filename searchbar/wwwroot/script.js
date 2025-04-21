// Fetch and render autocomplete suggestions for current input
async function fetchSuggestions() {
    let query = document.getElementById("search-box").value;
    if (query.length === 0) {
        document.getElementById("suggestions").innerHTML = "";
        return;
    }
    let response = await fetch(`/search?q=${query}`);
    let suggestions = await response.json();
    let suggestionBox = document.getElementById("suggestions");
    console.log('[autocomplete] suggestions:', suggestions);
    suggestionBox.innerHTML = "";
    suggestions.forEach(word => {
        let li = document.createElement("li");
        let p = document.createElement("p");
        let p2 = document.createElement("p");
        p.innerText = word;
        p2.innerText = "×";
        p2.style.color = "#e53e3e";
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

// Insert current search box value into the trie
async function insertWord() {
    let word = document.getElementById("search-box").value;
    if (!word) return;

    await fetch("/insert", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ word: word })
    });

    document.getElementById("search-box").value = "";
    document.getElementById("search-box").focus();
    fetchSuggestions();
}

// Remove word from trie and update suggestion list
async function deleteWord(word) {
    console.log('[autocomplete] deleting word:', word);
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

