
async function OpenMoreInfoModal(name) {
    const PokemonResponse = await fetch(`https://pokeapi.co/api/v2/pokemon/${name}`);

    if (PokemonResponse.ok) {
        let pokemon = await PokemonResponse.json();
        
        const qrResponse = await fetch('https://localhost:7066/qr', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(pokemon)
        });

        if (qrResponse.ok) {
            const qrImage = document.querySelector('#qrImage');
            var myModal = new bootstrap.Modal(document.getElementById('exampleModal'), {
                keyboard: true
            })

            qrImage.src = await qrResponse.text();

            myModal.show();
        }
    }
}
