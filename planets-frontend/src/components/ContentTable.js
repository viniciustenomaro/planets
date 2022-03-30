import { useEffect, useState } from "react";
import axios from "axios";
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';
import KeyboardArrowLeft from '@mui/icons-material/KeyboardArrowLeft';
import KeyboardArrowRight from '@mui/icons-material/KeyboardArrowRight';

const ContentTable = () => {
    const [items, setItems] = useState([]);
    const [page, setPage] = useState(1);

    useEffect(() => {
        const fetch = async () => {
            const result = await axios(
                `https://localhost:5001/api/planets?page=${page}`
            );

            setItems(result.data);
        };

        fetch();
    }, [page]);

    return (
        <TableContainer component={Paper}>
            <Table sx={{ minWidth: 650 }} size="small" aria-label="a dense table">
                <TableHead>
                    <TableRow>
                        <TableCell align="center">Name</TableCell>
                        <TableCell align="center">Population</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {items.map((row) => (
                        <TableRow
                            key={row.name}
                            sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                        >
                            <TableCell align="center">
                                {row.name}
                            </TableCell>
                            <TableCell align="center">{row.population}</TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    );
}

export default ContentTable;