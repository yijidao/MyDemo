const DataGrid = {
    props: ['itemsSource', 'columns'],
    template: `
    <table >
        <thead>
            <tr>
                <th v-for="column in columns" >
                    {{column.Header}}
                </th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="item in itemsSource">
                
                <td v-for="column in columns" >
                    {{item[column.Binding]}}
                </td>
            </tr>
        </tbody>
    </table>
    `,
}